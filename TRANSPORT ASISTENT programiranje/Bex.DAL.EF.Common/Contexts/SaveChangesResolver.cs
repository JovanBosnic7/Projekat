using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;



namespace Bex.DAL.EF.Common
{
    public class SaveChangesResolver : ISaveChangesResolver
    {
        public SaveChangesResolver()
            : this(new Dictionary<string, ICorrector>(), new Dictionary<string, IValidator>())
        { }
        public SaveChangesResolver(
            IDictionary<string, ICorrector> correctors,
            IDictionary<string, IValidator> validators)
        {
            Correctors = correctors;
            Validators = validators;
        }

        public ICorrector GetCorrector(DbEntityEntry entityEntry)
        {
            ICorrector corrector = null;
            var entityName = entityEntry?.Entity?.GetType().Name; // FullName if you needed

            if (entityName != null)
            { Correctors.TryGetValue(entityName, out corrector); }

            return corrector ?? CreateCorrector(entityName);
        }

        public IValidator GetValidator(DbEntityEntry entityEntry)
        {
            IValidator validator = null;
            var entityName = entityEntry?.Entity?.GetType().Name; // FullName if you needed

            if (entityName != null)
            { Validators.TryGetValue(entityName, out validator); }

            return validator ?? CreateValidator(entityName);
        }

        private ICorrector CreateCorrector(string entityName)
        {
            ICorrector corrector = null;

            switch (entityName)
            {
                case "Kontakt":
                    //corrector = Activator.CreateInstance(typeof(KontaktCorrector)) as ICorrector;
                    break;
                //case "Post":
                //case "Comment":
                //    corrector = Activator.CreateInstance(typeof(EntryCorrector)) as ICorrector;
                //    break;
                default:
                    break;
            }

            if (entityName != null && corrector != null)
            { Correctors.Add(entityName, corrector); }

            return corrector;
        }

        private IValidator CreateValidator(string entityName)
        {
            IValidator validator = null;

            switch (entityName)
            {
                case "Kontakt":
                    //validator = Activator.CreateInstance(typeof(KontaktValidator)) as IValidator; ;
                    break;
                //case "Post":
                //    validator = Activator.CreateInstance(typeof(PostValidator)) as IValidator;
                //    break;
                default:
                    break;
            }

            if (entityName != null && validator != null)
            { Validators.Add(entityName, validator); }

            return validator;
        }

        private IDictionary<string, ICorrector> Correctors { get; }
        private IDictionary<string, IValidator> Validators { get; }
    }
}
