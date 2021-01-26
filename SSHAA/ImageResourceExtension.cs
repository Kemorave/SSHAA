using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SSHAA
{
    [Preserve(AllMembers = true)]
    [Xamarin.Forms.ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public static Dictionary<string, ImageSource> Images = new Dictionary<string, ImageSource>();
        public static Assembly CurrentAssembly { get; set; }
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            string link = Source?.Replace('\\', '.');
            Xamarin.Forms.ImageSource image = null;

            try
            {
                if (Images.TryGetValue(Source, out image))
                {
                    return image;
                }
                if (string.IsNullOrEmpty(Source))
                {
                    return null;
                }
                if (CurrentAssembly == null)
                {
                    image = Xamarin.Forms.ImageSource.FromResource(link);
                }
                else
                {
                    image = Xamarin.Forms.ImageSource.FromResource(link, CurrentAssembly);
                }
                if (image == null)
                {
                    image = Xamarin.Forms.ImageSource.FromFile(@"C:\\Users\\Hema\\source\\repos\\SSHAA\\" + Source);
                }
                if (Source != null)
                {
                    Images.Add(Source, image);
                }

            }
            catch (Exception)
            {

            }
            return image;
        }
    }
}