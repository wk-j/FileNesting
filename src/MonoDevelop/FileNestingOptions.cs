
using MonoDevelop.Core;

namespace MadsKristensen.FileNesting
{
    static class FileNestingOptions
    {
        public static bool EnableAutoNesting
        {
            get { return PropertyService.Get("EnableAutoNesting", false); }
            set { PropertyService.Set("EnableAutoNesting", value); }
        }

        public static bool EnableExtensionRule
        {
            get { return PropertyService.Get("EnableExtensionRule", true); }
            set { PropertyService.Set("EnableExtensionRule", value); }
        }

        //public bool EnablePathSegmentRule { get; set; }
        //public bool EnableBundleRule { get; set; }
        //public bool EnableSpriteRule { get; set; }
        //public bool EnableKnownFileTypeRule { get; set; }
        //public bool EnableVsDocRule { get; set; }
        //public bool EnableInterfaceImplementationRule { get; set; }
    }
}

