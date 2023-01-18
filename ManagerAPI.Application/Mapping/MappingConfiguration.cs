using ManagerAPI.Application.FileArea.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Application.Mapping
{
    public static class MappingConfiguration
    {
        public static IEnumerable<AutoMapper.Profile> GetMappingProfile()
        {
            return new AutoMapper.Profile[]
            {
                new FileMappingProfile()
                //new TorrentMappingProfile()
            };
        }
    }
}
