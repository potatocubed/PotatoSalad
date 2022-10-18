using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoSalad.Code
{
    public class Violence
    {
        // Bump combat.
        // You bump.
        // You make a skill check.
        // You do damage based on your weapon.
        // You can make an offhand attack if your offhand has an 'unarmed' weapon in it. But it's at a -10 penalty.

        // Your skill ranges from 0-20.
        // The standard target is 11.
        // Roll 1d20 + skill.
        // Bonuses and penalties can apply.
        // If you fail the skill check, you get a check. After five checks, skill goes up.

        public void MakeBumpAttack(Mobile attacker, Mobile defender)
        {
            // get the weapon
            Item mainweapon;
            Item offweapon;

            if (attacker.inventory.mainhand != null)
            {
                mainweapon = attacker.inventory.mainhand;
            }
            else
            {
                mainweapon = Game.EmptyHand;
            }

            if (!mainweapon.type.ToLower().Contains("2h"))
            {
                if (attacker.inventory.offhand != null)
                {
                    offweapon = attacker.inventory.offhand;
                }
                else
                {
                    offweapon = Game.EmptyHand;
                }
            }
            else
            {
                offweapon = null;
            }

            // make the mainhand attack
            Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString(attacker.name)} attacks {Game.GAPI.CapitaliseString(defender.name)} with {mainweapon.name}.");
            if (MakeAttackRoll(mainweapon, attacker))
            {
                // HIT!
            }
            else
            {
                // MISS!
            }

            if(offweapon != null)
            {
                Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString(attacker.name)} attacks {Game.GAPI.CapitaliseString(defender.name)} with {offweapon.name}.");
                if (MakeAttackRoll(offweapon, attacker))
                {
                    // HIT!
                }
                else
                {
                    // MISS!
                }
            }
        }

        private bool MakeAttackRoll(Item weapon, Mobile attacker, int targetNumber = 11)
        {
            string skill = GetSkillFromWeapon(weapon.type);
            int skillRating = attacker.GetSkillRatingBySkill(skill);

            // roll dice vs target number
            int roll = Game.Dice.XdY(1, 20) + skillRating;
            if (roll >= targetNumber)
            {
                // HIT!
                Game.GAPI.RenderText("Hit!");
                return true;
            }
            else
            {
                // MISS!
                Game.GAPI.RenderText("Miss!");
                if (attacker.GetType().Name == "Player")
                {
                    // We only care about practice for the PC.
                    attacker.AddSkillCheck(skill);
                }
                return false;
            }
        }

        private string GetSkillFromWeapon(string weaponType)
        {
            // The skill the weapon uses is always SECOND in the type description.
            // (The first bit is 'weapon' for its broad type.)

            // TODO
            // I could probably generalise this out by adding an integer which looks for the nth entry
            // in the type string.

            int x = 0;
            int x2 = 0;
            string result = "";
            x = weaponType.IndexOf("-");
            x2 = weaponType.IndexOf("-", x + 1);
            if (x2 > 0)
            {
                result = weaponType.Substring(x + 1, (x2 - x) - 1);
            }
            else
            {
                result = weaponType.Substring(x + 1);
            }
            return result;
        }
    }
}
