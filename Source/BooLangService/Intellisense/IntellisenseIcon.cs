namespace Boo.BooLangService.Intellisense
{
    public enum IntellisenseIcon
    {
        // Each icon type has 6 versions, corresponding to the following
        // access types.
        AccessPublic = 0,
        AccessInternal = 1,
        AccessFriend = 2,
        AccessProtected = 3,
        AccessPrivate = 4,
        AccessShortcut = 5,

        Base = 6,

        Class = Base * 0,
        Constant = Base * 1,
        Delegate = Base * 2,
        Enumeration = Base * 3,
        EnumMember = Base * 4,
        Event = Base * 5,
        Exception = Base * 6,
        Field = Base * 7,
        Interface = Base * 8,
        Macro = Base * 9,
        Map = Base * 10,
        MapItem = Base * 11,
        Method = Base * 12,
        OverloadedMethod = Base * 13,
        Module = Base * 14,
        Namespace = Base * 15,
        Operator = Base * 16,
        Property = Base * 17,
        Struct = Base * 18,
        Template = Base * 19,
        Typedef = Base * 20,
        Type = Base * 21,
        Union = Base * 22,
        Variable = Base * 23,
        ValueType = Base * 24,
        Intrinsic = Base * 25,
        Object = Base * 26,
        MiscIcons = Base * 27,

        // Miscellaneous icons with one icon for each type.
        Reference = MiscIcons + 0,
        Library = MiscIcons + 1,
        ResourceFile = MiscIcons + 2,
        WebFile = MiscIcons + 3,
        VBProject = MiscIcons + 4,
        WebProject = MiscIcons + 5,
        CPPProject = MiscIcons + 6,
        DialogID = MiscIcons + 7,
        Dialog = MiscIcons + 8,
        OpenFolder = MiscIcons + 9,
        ClosedFolder = MiscIcons + 10,
        Arrow = MiscIcons + 11,
        Error = MiscIcons + 12,
        GrayedClass = MiscIcons + 13,
        GrayedPrivateMethod = MiscIcons + 14,
        GrayedProtectedMethod = MiscIcons + 15,
        GrayedMethod = MiscIcons + 16,
        MiscDocument = MiscIcons + 17
    }
}