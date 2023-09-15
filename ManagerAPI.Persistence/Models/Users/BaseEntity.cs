using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Persistence.Models.Users
{
    public abstract class BaseEntity //: IEntity
    {
        /// <summary>
        ///		The record ID
        /// </summary>
        /// <remarks>
        ///		Set by Raven Client. Can be temporarily null before passed to the DocumentSession.Store() method
        /// </remarks>
        public string Id { get; protected set; } = null!;
    }
}
