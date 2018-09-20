using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad
{
    class MonsterPopulater
    {
        private string GenerateUniqueID(string name, List<Mobile> mobArray)
        {
            // Takes a mobile's name and appends an ever-increasing number to it
            // until the ID is unique in the current mobile array.
            int iteration = 0;
            string testName = name + "-" + iteration.ToString();

            List<string> idList = new List<string>();

            foreach (Mobile m in mobArray)      // TODO: There's got to be a more efficient method than this.
            {
                idList.Add(m.id);
            }

            while (idList.Contains(testName))
            {
                iteration++;
                testName = name + "-" + iteration.ToString();
            }
            return testName;
        }

        public void MonsterSetUp(string monType, Mobile mon)
        {
            // TODO
            // This assigns properties to mon based on the string fed into monType.
            // If no string is fed in, we get 'generic monster'.

            // monType is also going to match an identifying string in the XML,
            // and form the basis of the unique ID.
        }
    }
}
