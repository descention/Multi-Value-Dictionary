using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace dictionary_crud_cli
{
    [Verb("keys", HelpText = "Returns all the keys in the dictionary.  Order is not guaranteed.")]
    internal class KeysOption { }

    [Verb("members", HelpText = "Returns the collection of strings for the given key.  Return order is not guaranteed.  Returns an error if the key does not exists.")]
    internal class MembersOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }
    }

    [Verb("add", HelpText = "Adds a member to a collection for a given key. Displays an error if the member already exists for the key.")]
    internal class AddOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }

        [Value(1, MetaName = "Member")]
        public string Member { get; set; }
    }

    [Verb("remove", HelpText = "Removes a member from a key.  If the last member is removed from the key, the key is removed from the dictionary. If the key or member does not exist, displays an error.")]
    internal class RemoveOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }

        [Value(1, MetaName = "Member")]
        public string Member { get; set; }
    }

    [Verb("removeall", HelpText = "Removes all members for a key and removes the key from the dictionary. Returns an error if the key does not exist.")]
    internal class RemoveAllOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }
    }

    [Verb("clear", HelpText = "Removes all keys and all members from the dictionary.")]
    internal class ClearOption { }

    [Verb("keyexists", HelpText = "Returns whether a key exists or not.")]
    internal class KeyExistsOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }
    }

    [Verb("memberexists", HelpText = "Returns whether a member exists within a key.  Returns false if the key does not exist.")]
    internal class MemberExistsOption
    {
        [Value(0, MetaName = "Key")]
        public string Key { get; set; }

        [Value(1, MetaName = "Member")]
        public string Member { get; set; }
    }

    [Verb("allmembers", HelpText = "Returns all the members in the dictionary.  Returns nothing if there are none. Order is not guaranteed.")]
    internal class AllMembersOption { }

    [Verb("items", HelpText = "Returns all keys in the dictionary and all of their members.  Returns nothing if there are none.  Order is not guaranteed.")]
    internal class ItemsOption { }

    [Verb("union")]
    internal class UnionOption { 
    
        [Value(0, MetaName = "KeyA")]
        public string KeyA { get; set; }
        [Value(1, MetaName = "KeyB")]
        public string KeyB { get; set; }
    }
}
