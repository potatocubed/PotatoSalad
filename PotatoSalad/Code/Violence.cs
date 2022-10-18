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
                int damage = MakeDamageRoll(mainweapon, attacker, defender);
                Game.GAPI.RenderText($"{damage} damage!");
                defender.DamageHealth(damage);
            }
            else
            {
                // MISS!
                // The skill-rating stuff should already be included.
            }

            if(offweapon != null && defender.health > 0)
            {
                Game.GAPI.RenderText($"{Game.GAPI.CapitaliseString(attacker.name)} attacks {Game.GAPI.CapitaliseString(defender.name)} with {offweapon.name}.");
                // -5 penalty on off-hand attacks
                if (MakeAttackRoll(offweapon, attacker, 16))
                {
                    // HIT!
                    int damage = MakeDamageRoll(offweapon, attacker, defender);
                    Game.GAPI.RenderText($"{damage} damage!");
                    defender.DamageHealth(damage);
                }
                else
                {
                    // MISS!
                    // No skill-ups.
                }
            }
        }

        private int MakeDamageRoll(Item weapon, Mobile attacker, Mobile defender)
        {
            // Attacker and defender are included for future shenanigans.
            // For our super-basic system right now we just want damage.
            // And damage type.

            // Damage lines start with a value (a die or a flat value)
            // and then a bunch of tags which can be in any order.

            string[] wTags = weapon.damage.Split('-');

            string damage = wTags[0];
            int dam = -1;
            int.TryParse(damage, out dam);
            // That catches flat damage.

            if(dam == 0)
            {
                // Not flat damage.
                string[] damExpression = damage.Split('d');

                int x;
                int y;
                if (int.TryParse(damExpression[0], out x) &&
                    int.TryParse(damExpression[1], out y))
                {
                    Game.GAPI.RenderText($"Rolling {x}d{y} for damage...");
                    dam = Game.Dice.XdY(x, y);
                }
                else
                {
                    // It's all fucked.
                    Game.GAPI.RenderText("Damage output nonsensical!");
                    dam = 1;
                }
            }

            return dam;
        }

        private bool MakeAttackRoll(Item weapon, Mobile attacker, int targetNumber = 11, bool skillCheck = true)
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
                // We only care about practice for the PC.
                if (attacker.GetType().Name == "Player")
                {
                    if (skillCheck)
                    {
                        attacker.AddSkillCheck(skill);
                    }
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
