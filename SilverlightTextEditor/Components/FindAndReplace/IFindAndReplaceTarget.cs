using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Ijv.Redstone.TextEditor
{
    public interface IFindAndReplaceTarget
    {
        string Text { get; set; }
        string SelectedText { get; set; }
        int SelectionStart { get; set; }
        int SelectionLength { get; set; }
    }
}
