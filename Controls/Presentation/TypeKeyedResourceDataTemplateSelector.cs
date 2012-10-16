using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Selects a <see cref="DataTemplate"/> by traversing the visual tree and selecting <see cref="DataTemplate"/> resources
    /// with a key that's an assembly qualified type name covariant to the content's type.
    /// </summary>
    public class TypeKeyedResourceDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Returns the data template in application resources that best matches the item type.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a DataTemplate or null reference.</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null || container == null)
            {
                return null;
            }

            DataTemplate selectedDataTemplate = null;
            Type dataType = item.GetType();

            // recursively check framework element resources in the visual tree.
            DependencyObject current = container;

            while (current != null)
            {
                FrameworkElement element = current as FrameworkElement;

                if (element != null)
                {
                    DataTemplate dataTemplate = this.GetDataTemplate(element.Resources, dataType);

                    if (dataTemplate != null)
                    {
                        return dataTemplate;
                    }
                }

                current = VisualTreeHelper.GetParent(current);
            }

            // if the visual tree search failed, check the application resources if a matching template 
            DataTemplate applicationDataTemplate = this.GetDataTemplate(Application.Current.Resources, dataType);

            if (applicationDataTemplate != null)
            {
                selectedDataTemplate = applicationDataTemplate;
            }

            return selectedDataTemplate;
        }

        /// <summary>
        /// Gets the best data template in the resource dictionary given the specified type.
        /// </summary>
        /// <param name="resources">The resource dictionary.</param>
        /// <param name="forType">The type.</param>
        /// <returns>The best data template in the resource dictionary given the specified type.</returns>
        private DataTemplate GetDataTemplate(ResourceDictionary resources, Type forType)
        {
            // Preconditions

            Argument.IsNotNull("resources", resources);
            Argument.IsNotNull("forType", forType);

            // Implementation

            Dictionary<Type, DataTemplate> dataTemplates = new Dictionary<Type, DataTemplate>();

            this.FillDataTemplatesDictionaryFromResourcesForType(dataTemplates, resources, forType);

            var variances = from dt in dataTemplates
                            let variance = GetVariance(forType, dt.Key, 100, 0, false)
                            where variance != null
                            orderby variance
                            select dt;

            var bestMatch = variances.FirstOrDefault();

            if (bestMatch.Key != null)
            {
                return bestMatch.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fill a data template dictionary with all relevant data templates from the resource dictionary for the specified type.
        /// </summary>
        /// <param name="resources">The resource dictionary.</param>
        /// <param name="dataTemplates">The data templates dictionary to fill.</param>
        /// <param name="forType">The type.</param>
        private void FillDataTemplatesDictionaryFromResourcesForType(Dictionary<Type, DataTemplate> dataTemplates, ResourceDictionary resources, Type forType)
        {
            Type currentType = forType;

            // TODO : create a translation for the types.
            string typeString = currentType.ToString().Replace("Ijv.Redstone.Design.", "design:");

            while (currentType != null)
            {
                if (resources[typeString] is DataTemplate && !dataTemplates.ContainsKey(currentType))
                {
                    dataTemplates.Add(currentType, resources[typeString] as DataTemplate);
                }

                foreach (Type interfaceType in currentType.GetInterfaces())
                {
                    this.FillDataTemplatesDictionaryFromResourcesForType(dataTemplates, resources, interfaceType);
                }

                currentType = currentType.BaseType;
            }
        }

        private static int? GetVariance(Type source, Type input, int classOffset, int interfaceOffset, bool checkContravariance)
        {
            // preconditions

            Argument.IsNotNull("source", source);
            Argument.IsNotNull("input", input);

            // implementation

            int covariance = 0;

            Type current = source;

            while (current != null)
            {
                if (input.IsInterface)
                {
                    if (current == input)
                    {
                        if (current == source)
                        {
                            return 0;
                        }
                        else
                        {
                            return covariance + interfaceOffset;
                        }
                    }

                    List<int> interfaceVariances = new List<int>();
            
                    foreach (Type interfaceDeclaration in current.GetInterfaces())
                    {
                        int? variance = GetVariance(interfaceDeclaration, input, 0, 0, checkContravariance);

                        if (variance != null)
                        {
                            interfaceVariances.Add(variance.Value + 1);
                        }
                    }

                    if (interfaceVariances.Count > 0)
                    {
                        // determine the minimum variance
                        int? minimumVariance = null;
                        foreach (int variance in interfaceVariances)
                        {
                            if (minimumVariance != null)
                            {
                                if (variance < minimumVariance)
                                {
                                    minimumVariance = variance;
                                }
                            }
                            else
                            {
                                minimumVariance = variance;
                            }
                        }

                        return minimumVariance + covariance + interfaceOffset;
                    }
                }
                else
                {
                    if (current == input)
                    {
                        if (current == source)
                        {
                            return 0;
                        }
                        else
                        {
                            return covariance + classOffset;
                        }
                    }
                }

                covariance++;

                current = current.BaseType;
            }

            if (checkContravariance)
            {
                int? contravariance = GetVariance(input, source, classOffset, interfaceOffset, false);

                if (contravariance != null)
                {
                    return contravariance * -1;
                }
            }

            return null;
        }

    }
}
