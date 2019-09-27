namespace NETGame.CSharp.Entities
{
    public class Skill
    {
        private int _id;
        private int _niveau;
        private string _skillName;

        public int Id { get => _id; set => _id = value; }
        public int Niveau { get => _niveau; set => _niveau = value; }
        public string SkillName { get => _skillName; set => _skillName = value; }

        public Skill()
        {

        }
        public Skill(int skillId, int skillLevel , string skillName)
        {

        }
    }
}