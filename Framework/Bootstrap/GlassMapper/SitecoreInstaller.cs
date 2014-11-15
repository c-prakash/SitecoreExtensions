/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/
//-CRE-

using Glass.Mapper;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using Microsoft.Practices.Unity;
using Framework.Bootstrap.Container.UnityExtensions;

namespace Framework.Bootstrap.GlassMapper
{
    /// <summary>
    /// Class SitecoreInstaller
    /// </summary>
    public class SitecoreInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Gets or sets the data mapper installer.
        /// </summary>
        /// <value>
        /// The data mapper installer.
        /// </value>
        public IUnityInstaller DataMapperInstaller { get; set; }

        /// <summary>
        /// Gets or sets the query parameter installer.
        /// </summary>
        /// <value>
        /// The query parameter installer.
        /// </value>
        public IUnityInstaller QueryParameterInstaller { get; set; }

        /// <summary>
        /// Gets or sets the data mapper task installer.
        /// </summary>
        /// <value>
        /// The data mapper task installer.
        /// </value>
        public IUnityInstaller DataMapperTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the configuration resolver task installer.
        /// </summary>
        /// <value>
        /// The configuration resolver task installer.
        /// </value>
        public IUnityInstaller ConfigurationResolverTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the objection construction task installer.
        /// </summary>
        /// <value>
        /// The objection construction task installer.
        /// </value>
        public IUnityInstaller ObjectionConstructionTaskInstaller { get; set; }

        /// <summary>
        /// Gets or sets the object saving task installer.
        /// </summary>
        /// <value>
        /// The object saving task installer.
        /// </value>
        public IUnityInstaller ObjectSavingTaskInstaller { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        public SitecoreInstaller()
            : this(new Config())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SitecoreInstaller(Config config)
        {
            Config = config;
            DataMapperInstaller = new DataMapperInstaller(config);
            QueryParameterInstaller = new QueryParameterInstaller(config);
            DataMapperTaskInstaller = new DataMapperTaskInstaller(config);
            ConfigurationResolverTaskInstaller = new ConfigurationResolverTaskInstaller(config);
            ObjectionConstructionTaskInstaller = new ObjectionConstructionTaskInstaller(config);
            ObjectSavingTaskInstaller = new ObjectSavingTaskInstaller(config);
        }


        /// <summary>
        /// Performs the installation in the IUnityInstaller.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            container.Install(DataMapperInstaller,
                            QueryParameterInstaller,
                            DataMapperTaskInstaller,
                            ConfigurationResolverTaskInstaller,
                            ObjectionConstructionTaskInstaller,
                            ObjectSavingTaskInstaller);

            container.RegisterInstance(typeof(Glass.Mapper.Sc.Config), Config);
        }
    }

    /// <summary>
    /// Installs the components descended from AbstractDataMapper. These are used to map data
    /// to and from the CMS.
    /// </summary>
    public class DataMapperInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            container
            .RegisterType<AbstractDataMapper, SitecoreIgnoreMapper>()
            .RegisterType<AbstractDataMapper, SitecoreChildrenCastMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldBooleanMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldDateTimeMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldDecimalMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldDoubleMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldEnumMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldFileMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldFloatMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldGuidMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldHtmlEncodingMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldIEnumerableMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldImageMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldIntegerMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldLinkMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldLongMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNameValueCollectionMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableDateTimeMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableDoubleMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableDecimalMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableFloatMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableGuidMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldNullableIntMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldRulesMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldStreamMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldStringMapper>()
            .RegisterType<AbstractDataMapper, SitecoreFieldTypeMapper>()
            .RegisterType<AbstractDataMapper, SitecoreIdMapper>()
            .RegisterType<AbstractDataMapper, SitecoreItemMapper>()
            .RegisterType<AbstractDataMapper, SitecoreInfoMapper>()
            .RegisterType<AbstractDataMapper, SitecoreNodeMapper>()
            .RegisterType<AbstractDataMapper, SitecoreLinkedMapper>()
            .RegisterType<AbstractDataMapper, SitecoreParentMapper>()
            .RegisterType<AbstractDataMapper, SitecoreQueryMapper>(new InjectionConstructor(container.ResolveAll<ISitecoreQueryParameter>()));
        }
    }

    /// <summary>
    /// Used by the SitecoreQueryMapper to replace placeholders in a query
    /// </summary>
    public class QueryParameterInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public QueryParameterInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            container
            .RegisterType<ISitecoreQueryParameter, ItemDateNowParameter>()
            .RegisterType<ISitecoreQueryParameter, ItemEscapedPathParameter>()
            .RegisterType<ISitecoreQueryParameter, ItemIdNoBracketsParameter>()
            .RegisterType<ISitecoreQueryParameter, ItemIdParameter>()
            .RegisterType<ISitecoreQueryParameter, ItemPathParameter>();
        }
    }

    /// <summary>
    /// Data Mapper Resolver Tasks -
    /// These tasks are run when Glass.Mapper tries to resolve which DataMapper should handle a given property, e.g.
    /// </summary>
    public class DataMapperTaskInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMapperTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public DataMapperTaskInstaller(Config config)
        {
            Config = config;
        }
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            // Tasks are called in the order they are specified.
            container.RegisterType<IDataMapperResolverTask, DataMapperStandardResolverTask>();
        }
    }

    /// <summary>
    /// Configuration Resolver Tasks - These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passsed.
    /// </summary>
    public class ConfigurationResolverTaskInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolverTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ConfigurationResolverTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            // These tasks are run when Glass.Mapper tries to find the configuration the user has requested based on the type passed, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return the MyClass configuration. 
            // Tasks are called in the order they are specified below.
            container
                .RegisterType<IConfigurationResolverTask, SitecoreItemResolverTask>()
                .RegisterType<IConfigurationResolverTask, MultiInterfaceResolverTask>()
                .RegisterType<IConfigurationResolverTask, TemplateInferredTypeTask>()
                .RegisterType<IConfigurationResolverTask, ConfigurationStandardResolverTask>()
                .RegisterType<IConfigurationResolverTask, ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>>();
        }
    }

    /// <summary>
    /// Object Construction Tasks - These tasks are run when an a class needs to be instantiated by Glass.Mapper.
    /// </summary>
    public class ObjectionConstructionTaskInstaller : IUnityInstaller
    {

        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectionConstructionTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ObjectionConstructionTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            //dynamic must be first
            container
                .RegisterType<IObjectConstructionTask, CreateDynamicTask>()
                .RegisterType<IObjectConstructionTask, SitecoreItemTask>()
                .RegisterType<IObjectConstructionTask, CreateMultiInferaceTask>()
                .RegisterType<IObjectConstructionTask, CreateConcreteTask>()
                .RegisterType<IObjectConstructionTask, CreateInterfaceTask>();
        }
    }

    /// <summary>
    /// Object Saving Tasks - These tasks are run when an a class needs to be saved by Glass.Mapper.
    /// </summary>
    public class ObjectSavingTaskInstaller : IUnityInstaller
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>
        /// The config.
        /// </value>
        public Config Config { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSavingTaskInstaller"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ObjectSavingTaskInstaller(Config config)
        {
            Config = config;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IUnityContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        public virtual void Install(IUnityContainer container)
        {
            // Tasks are called in the order they are specified below.
            container.RegisterType<IObjectSavingTask, StandardSavingTask>();
        }
    }
}
