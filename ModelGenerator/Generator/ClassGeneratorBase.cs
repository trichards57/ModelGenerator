using ModelGenerator.Model;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Generator
{
    internal abstract class ClassGeneratorBase
    {
        protected ClassGeneratorBase(OutputMode mode)
        {
            Mode = mode;
            GeneratorVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        protected string GeneratorVersion { get; }
        protected OutputMode Mode { get; }

        public abstract void CreateClass(Class model, StringBuilder output);

        public virtual void CreateClasses(Classes model, StringBuilder output)
        {
            var cls = model.Items.AsEnumerable();

            switch (Mode)
            {
                case OutputMode.Create:
                    cls = cls.Where(c => c.GenerateCreateModel);
                    break;

                case OutputMode.Details:
                    cls = cls.Where(c => c.GenerateDetailModel);
                    break;

                case OutputMode.Summary:
                    cls = cls.Where(c => c.GenerateSummaryModel);
                    break;

                case OutputMode.Update:
                    cls = cls.Where(c => c.GenerateUpdateModel);
                    break;
            }

            foreach (var cl in cls)
            {
                CreateClass(cl, output);
                output.AppendLine();
            }
        }
    }
}
