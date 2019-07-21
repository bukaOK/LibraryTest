using AutoMapper;
using LibraryTest.BLL.MapProfiles;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LibraryTest.Tests.Helpers
{
    public static class MocksHelper
    {
        private static IMapper mapper;
        public static IMapper GetMapper()
        {
            if (mapper != null)
                return mapper;
            var mapConfig = new MapperConfiguration(mc =>
            {
                // Скан сборки для получения профилей (LibraryTest.BLL/MapProfiles)
                mc.AddMaps(Assembly.GetAssembly(typeof(ClientProfile)));
            });
            mapper = mapConfig.CreateMapper();
            return mapper;
        }

        public static ILogger<T> GetLogger<T>()
        {
            var logger = Mock.Of<ILogger<T>>();
            return logger;
        }
    }
}
