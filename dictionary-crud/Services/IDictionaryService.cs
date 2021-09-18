using System;
using System.Collections.Generic;
using System.Text;

namespace dictionary_crud.Services
{
    public interface IDictionaryService
    {
        public IEnumerable<string> Keys();
        public IEnumerable<string> Members(string key);
        public void Add(string key, string member);
        public void Remove(string key, string member);
        public void RemoveAll(string key);
        public void Clear();
        public bool KeyExists(string key);
        public bool MemberExists(string key, string member);
        public IEnumerable<string> AllMembers();
        public IEnumerable<(string key, string member)> Items();
    }
}
