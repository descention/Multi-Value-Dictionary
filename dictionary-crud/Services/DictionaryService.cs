using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dictionary_crud.Services
{
    public class DictionaryService: IDictionaryService
    {
        private IDictionary<string, IList<string>> dataset = new Dictionary<string, IList<string>>();

        /// <summary>
        /// Returns all the keys in the dictionary.  Order is not guaranteed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Keys() => dataset.Keys;

        /// <summary>
        /// Returns the collection of strings for the given key.  Return order is not guaranteed.  Returns an error if the key does not exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> Members(string key)
        {
            if (dataset.TryGetValue(key, out IList<string> valList))
            {
                return valList;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"{nameof(key)} does not exist");
            }
        }

        /// <summary>
        /// Adds a member to a collection for a given key. Displays an error if the member already exists for the key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public void Add(string key, string member)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"{nameof(key)} must not be empty", nameof(key));
            if (string.IsNullOrWhiteSpace(member))
                throw new ArgumentException($"{nameof(member)} must not be empty", nameof(member));

            if (dataset.ContainsKey(key))
            { // we have the key
                if (dataset.TryGetValue(key, out IList<string> valueList))
                { // we have a list
                    if (valueList.Contains(member))
                    { // value is already in the list
                        throw new InvalidOperationException("ERROR, member already exists for key");
                    }
                    else
                    { // add the value
                        valueList.Add(member);
                    }
                }
                else
                { // we need to make the list
                    dataset[key] = new List<string>() { member };
                }
            }
            else
            { // add the key and the list
                dataset.Add(key, new List<string> { member });
            }
        }

        /// <summary>
        /// Removes a member from a key.  If the last member is removed from the key, the key is removed from the dictionary. If the key or member does not exist, displays an error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        public void Remove(string key, string member)
        {
            if (!dataset.ContainsKey(key))
                throw new InvalidOperationException("key does not exist");

            // if we can't get the value (no list) OR we can't remove the member
            if (!dataset.TryGetValue(key, out IList<string> values) || !values.Remove(member))
                throw new InvalidOperationException("member does not exist");

            if (dataset.ContainsKey(key) && dataset[key].Count == 0)
                dataset.Remove(key);
        }

        /// <summary>
        /// Removes all members for a key and removes the key from the dictionary. Returns an error if the key does not exist.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveAll(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (dataset.ContainsKey(key))
                dataset.Remove(key);
            else
                throw new InvalidOperationException("key does not exist");
        }

        /// <summary>
        /// Removes all keys and all members from the dictionary.
        /// </summary>
        public void Clear()
        {
            dataset = new Dictionary<string, IList<string>>();
        }

        /// <summary>
        /// Returns whether a key exists or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return dataset.ContainsKey(key);
        }

        /// <summary>
        /// Returns whether a member exists within a key.  Returns false if the key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool MemberExists(string key, string member)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"{nameof(key)} must not be empty", nameof(key));
            if (string.IsNullOrWhiteSpace(member))
                throw new ArgumentException($"{nameof(member)} must not be empty", nameof(member));

            return dataset.TryGetValue(key, out IList<string> values) && values.Contains(member);
        }

        /// <summary>
        /// Returns all the members in the dictionary.  Returns nothing if there are none. Order is not guaranteed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> AllMembers() => dataset.SelectMany(t => t.Value);

        /// <summary>
        /// Returns all keys in the dictionary and all of their members.  Returns nothing if there are none.  Order is not guaranteed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(string key, string member)> Items()
        {
            foreach(var key in Keys())
            {
                foreach(var member in dataset[key])
                {
                    yield return (key, member);
                }
            }
        }

        /// <summary>
        /// Returns members that exist for either key, doesn't return duplicates
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public IEnumerable<string> Union(string keyA, string keyB) => Members(keyA).Union(Members(keyB));

        public IEnumerable<string> Except(string keyA, string keyB)
        {
            var A = Members(keyA);
            var B = Members(keyB);

            var first = A.Except(B);
            var second = B.Except(A);

            return first.Union(second);
        }
    }
}
