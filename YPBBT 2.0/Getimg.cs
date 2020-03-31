using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace YPBBT_2._0
{
    class Getimg
    {
        public BitmapImage GETIMAGE(string link)
        {
            BitmapImage bitmap = new BitmapImage();
            var fullFilePath = link;


            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();          
            return bitmap;

        }
    }
}
