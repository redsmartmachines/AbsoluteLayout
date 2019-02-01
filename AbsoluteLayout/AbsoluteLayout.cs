using System;
using Xamarin.Forms;

namespace SmartMachines
{
    public class AbsoluteLayout : Xamarin.Forms.AbsoluteLayout
    {
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            foreach (View child in Children)
            {
                Rectangle rect = ComputeLayoutForRegion(child, new Size(width, height));
                rect.X += x;
                rect.Y += y;

                LayoutChildIntoBoundingRegion(child, rect);
            }
        }

        static Rectangle ComputeLayoutForRegion(View view, Size region)
        {
            var result = new Rectangle();

            SizeRequest sizeRequest;
            Rectangle bounds = GetLayoutBounds(view);
            AbsoluteLayoutFlags absFlags = GetLayoutFlags(view);
            bool widthIsProportional = (absFlags & AbsoluteLayoutFlags.WidthProportional) != 0;
            bool heightIsProportional = (absFlags & AbsoluteLayoutFlags.HeightProportional) != 0;
            bool xIsProportional = (absFlags & AbsoluteLayoutFlags.XProportional) != 0;
            bool yIsProportional = (absFlags & AbsoluteLayoutFlags.YProportional) != 0;

            if (widthIsProportional)
            {
                result.Width = Device.Info.DisplayRound(region.Width * bounds.Width);
            }
            else if (bounds.Width != AutoSize)
            {
                result.Width = bounds.Width;
            }

            if (heightIsProportional)
            {
                result.Height = Device.Info.DisplayRound(region.Height * bounds.Height);
            }
            else if (bounds.Height != AutoSize)
            {
                result.Height = bounds.Height;
            }

            if (!widthIsProportional && bounds.Width == AutoSize)
            {
                if (!heightIsProportional && bounds.Width == AutoSize)
                {
                    // Width and Height are auto
                    sizeRequest = view.Measure(region.Width, region.Height, MeasureFlags.IncludeMargins);
                    result.Width = sizeRequest.Request.Width;
                    result.Height = sizeRequest.Request.Height;
                }
                else
                {
                    // Only width is auto
                    sizeRequest = view.Measure(region.Width, result.Height, MeasureFlags.IncludeMargins);
                    result.Width = sizeRequest.Request.Width;
                }
            }
            else if (!heightIsProportional && bounds.Height == AutoSize)
            {
                // Only height is auto
                sizeRequest = view.Measure(result.Width, region.Height, MeasureFlags.IncludeMargins);
                result.Height = sizeRequest.Request.Height;
            }

            if (xIsProportional)
            {
                //result.X = Device.Info.DisplayRound((region.Width - result.Width) * bounds.X);
                result.X = Device.Info.DisplayRound(region.Width * bounds.X);
            }
            else
            {
                result.X = bounds.X;
            }

            if (yIsProportional)
            {
                //result.Y = Device.Info.DisplayRound((region.Height - result.Height) * bounds.Y);
                result.Y = Device.Info.DisplayRound(region.Height * bounds.Y);
            }
            else
            {
                result.Y = bounds.Y;
            }

            return result;
        }
    }
}
