
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

        public static bool EnablePathSegmentRule
        {
            get { return PropertyService.Get("EnablePathSegmentRule", true); }
            set { PropertyService.Set("EnablePathSegmentRule", value); }
        }

        public static bool EnableKnownFileTypeRule
        {
            get { return PropertyService.Get("EnableKnownFileTypeRule", true); }
            set { PropertyService.Set("EnableKnownFileTypeRule", value); }
        }

        public static bool EnableInterfaceImplementationRule
        {
            get { return PropertyService.Get("EnableInterfaceImplementationRule", false); }
            set { PropertyService.Set("EnableInterfaceImplementationRule", value); }
        }
    }
}

