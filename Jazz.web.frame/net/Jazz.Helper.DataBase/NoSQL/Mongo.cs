//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MongoDB.Driver;
//using MongoDB.Bson;
//using MongoDB.Driver.Builders;

//namespace Jazz.Helper.DataBase.NoSQL
//{
//    public class Mongo
//    {
//        private const string strconn = "mongodb://127.0.0.1:27017";

//        private const string dbName = "BeanDB";

//        public MongoDatabase DB
//        {
//            get
//            {
//                MongoServer server = new MongoDB.Driver.MongoClient(strconn).GetServer();
//                //MongoServer server = MongoDB.Driver.MongoServer.Create(strconn);
//                return server.GetDatabase(dbName);
//            }
//        }

//        public bool InsertBean<T>(string Set, T Bean)
//        {
//            try
//            {
//                MongoCollection col = this.DB.GetCollection(Set);
//                col.Insert<T>(Bean);
//                return true;
//            }
//            catch (Exception e)
//            {
//                return false;
//            }

//        }

//        public bool InsertBeans<T>(string Set, params T[] Beans)
//        {
//            try
//            {
//                MongoCollection col = this.DB.GetCollection(Set);
//                col.InsertBatch<T>(Beans);
//                return true;
//            }
//            catch (Exception e)
//            {
//                return false;
//            }
//        }

//        public bool UpdateBean(string Set, IMongoQuery query, UpdateDocument update)
//        {
//            try
//            {
//                MongoCollection col = this.DB.GetCollection(Set);
//                col.Update(query, update);
//                return true;
//            }
//            catch (Exception e)
//            {
//                return false;
//            }
//        }

//        public bool DeleteBeans(string Set, IMongoQuery query)
//        {
//            try
//            {
//                MongoCollection col = this.DB.GetCollection(Set);
//                col.Remove(query);
//                return true;
//            }
//            catch (Exception e)
//            {
//                return false;
//            }
//        }

//        public MongoCursor<T> QueryBeans<T>(string Set, IMongoQuery query)
//        {
//            MongoCollection col = this.DB.GetCollection(Set);
//            return col.FindAs<T>(query);

//        }

//        public MongoCursor QueryBeans(string Set, IMongoQuery query)
//        {
//            var col = this.DB.GetCollection(Set);
//            return col.Find(query);
            
//        }
//    }
//    public class MongoHelper<T> where T : class, new()
//    {
//        #region 基本属性
//        /// <summary>
//        /// 数据库连接
//        /// </summary>
//        private string connString = "mongodb://127.0.0.1:27017";
//        /// <summary>
//        /// 指定的数据库
//        /// </summary>
//        private string dbName = "dbName";
//        /// <summary>
//        /// 指定的集合
//        /// </summary>
//        private string colName = "colName";
//        /// <summary>
//        /// 创建数据连接
//        /// </summary>
//        static MongoServer server;
//        /// <summary>
//        /// 获取指定数据库
//        /// </summary>
//        static MongoDatabase db;
//        /// <summary>
//        /// 获取集合
//        /// </summary>
//        static MongoCollection<T> col;
//        #endregion

//        #region 构造函数
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        public MongoHelper()
//        {
//            Init(null, null, null);
//        }
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="connString">连接字符串</param>
//        public MongoHelper(string connString)
//        {
//            Init(connString, null, null);
//        }
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="connString">连接字符串</param>
//        /// <param name="dbName">数据库名</param>
//        public MongoHelper(string connString, string dbName)
//        {
//            Init(connString, dbName, null);
//        }
//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="connString">连接字符串</param>
//        /// <param name="dbName">数据库名</param>
//        /// <param name="colName">数据库表名</param>
//        public MongoHelper(string connString, string dbName, string colName)
//        {
//            Init(connString, dbName, colName);
//        }
//        #endregion

//        #region 初始化
//        /// <summary>
//        /// 初始化
//        /// </summary>
//        /// <param name="connString">连接字符串</param>
//        /// <param name="dbName">数据库</param>
//        /// <param name="colName">集合名</param>
//        public void Init(string connString = null, string dbName = null, string colName = null)
//        {
//            if (server == null)
//            {
//                server = new MongoClient(connString).GetServer();
//                db = server.GetDatabase(dbName);
//                col = db.GetCollection<T>(colName);
//            }
//        }
//        #endregion

//        #region 基本方法
//        /// <summary>
//        /// 添加
//        /// </summary>
//        /// <param name="T">T</param>
//        /// <returns></returns>
//        public WriteConcernResult Insert(T t)
//        {
//            return col.Insert(t);
//        }

//        /// <summary>
//        /// 根据bsonValue 删除
//        /// </summary>
//        /// <param name="bsonValue"></param>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public WriteConcernResult Remove(string bsonValue, string name = "_id")
//        {
//            //查询当前的文档
//            var query = Query.EQ(name, bsonValue);

//            //删除文档
//            return col.Remove(query);
//        }

//        /// <summary>
//        /// 更新
//        /// </summary>
//        /// <param name="t">泛型</param>
//        /// <param name="bsonValue">bsonValue的值</param>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public WriteConcernResult Update(T t, string bsonValue, string name = "_id")
//        {
//            //BsonDocument
//            var bd = BsonExtensionMethods.ToBsonDocument(t);
//            //query
//            var query = Query.EQ(name, bsonValue);

//            return col.Update(query, new UpdateDocument(bd));
//        }

//        /// <summary>
//        /// 获取某一文档
//        /// </summary>
//        /// <param name="bsonValue"></param>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public T GetObject(string bsonValue, string name = "_id")
//        {
//            //条件查询            
//            return col.FindOne(Query.EQ(name, bsonValue));
//        }

//        /// <summary>
//        /// 按条件查询
//        /// </summary>
//        /// <param name="pageInfo">分页的信息</param>
//        /// <returns></returns>
//        public MongoCursor<T> GetListByPage(IMongoQuery query, PageInfo pageInfo, out long record)
//        {
//            var result = col.Find(query).SetSkip((pageInfo.PageIndex - 1) * pageInfo.PageSize).SetLimit(pageInfo.PageSize);

//            if (pageInfo.Sort == null || pageInfo.Sort.Count() == 0)
//            { }
//            else
//            {
//                if (pageInfo.Direction == Direction.DESC)
//                {
//                    result = result.SetSortOrder(SortBy.Descending(pageInfo.Sort));
//                }
//                else
//                {
//                    result = result.SetSortOrder(SortBy.Ascending(pageInfo.Sort));
//                }
//            }
//            record = result.Count();

//            return result;
//        }

//        /// <summary>
//        /// 查询所有
//        /// </summary>
//        public MongoCursor<T> GetAll()
//        {
//            return col.FindAll();
//        }

//        /// <summary>
//        /// 创建索引
//        /// </summary>
//        /// <param name="kNames">索引的值</param>
//        public void EnsureIndex(string[] kNames)
//        {
//            var doc = new MongoDB.Driver.IndexKeysDocument();
//            foreach (var name in kNames)
//            {
//                doc.Add(name, 1);
//            }
//            col.EnsureIndex(doc);
//        }

//        /// <summary>
//        /// 创建索引
//        /// </summary>
//        /// <param name="kNames">索引的值</param>
//        /// <returns></returns>
//        public WriteConcernResult CreateIndex(string[] kNames)
//        {
//            var doc = new MongoDB.Driver.IndexKeysDocument();
//            foreach (var name in kNames)
//            {
//                doc.Add(name, 1);
//            }

//            return col.CreateIndex(doc);
//        }

//        /// <summary>
//        /// 获取索引
//        /// </summary>
//        /// <returns></returns>
//        public GetIndexesResult GetIndexes()
//        {
//            return col.GetIndexes();
//        }

//        /// <summary>
//        /// 删除文档
//        /// </summary>
//        /// <returns></returns>
//        public CommandResult Drop()
//        {
//            return col.Drop();
//        }

//        /// <summary>
//        /// 删除所有的索引
//        /// </summary>
//        /// <returns></returns>
//        public CommandResult DropAllIndexes()
//        {
//            return col.DropAllIndexes();
//        }

//        /// <summary>
//        /// 删除制定的索引
//        /// </summary>
//        /// <param name="names"></param>
//        /// <returns></returns>
//        public CommandResult DropIndex(string[] names)
//        {
//            return col.DropIndex(names);
//        }

//        /// <summary>
//        /// 删除制定的索引名称
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public CommandResult DropIndexByName(string name)
//        {
//            return col.DropIndexByName(name);
//        }

//        /// <summary>
//        /// 验证文档是否存在
//        /// </summary>
//        /// <returns></returns>
//        public bool Exists()
//        {
//            return col.Exists();
//        }

//        /// <summary>
//        /// 查询
//        /// </summary>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        public MongoCursor<T> sad(IMongoQuery query)
//        {
//            return col.Find(query);
//        }

//        /// <summary>
//        /// 查询
//        /// </summary>
//        /// <param name="documentType"></param>
//        /// <returns></returns>
//        public MongoCursor FindAllAs(Type documentType)
//        {
//            return col.FindAllAs(documentType);
//        }

//        /// <summary>
//        /// 查询并删除
//        /// </summary>
//        /// <param name="query">查询条件</param>
//        /// <param name="sortBy">排序方式</param>
//        /// <param name="update">更新</param>
//        /// <param name="returnNew">返回新的值</param>
//        /// <param name="upSert"></param>
//        /// <returns></returns>
//        public FindAndModifyResult FindAndModify(IMongoQuery query, IMongoSortBy sortBy, IMongoUpdate update, bool returnNew = false, bool upSert = false)
//        {
//            return col.FindAndModify(query, sortBy, update, returnNew, upSert);
//        }

//        /// <summary>
//        /// 查询并且删除
//        /// </summary>
//        /// <param name="query"></param>
//        /// <param name="sortBy"></param>
//        /// <returns></returns>
//        public FindAndModifyResult FindAndModify(IMongoQuery query, IMongoSortBy sortBy)
//        {
//            return col.FindAndRemove(query, sortBy);
//        }

//        /// <summary>
//        /// 系统会随机查询获取到满足条件的一条记录
//        /// </summary>
//        /// <returns></returns>
//        public T FindOne()
//        {
//            return col.FindOne();
//        }

//        /// <summary>
//        /// 根据Id获取第一个
//        /// </summary>
//        /// <param name="bsonValue"></param>
//        public T FindOneById(BsonValue bsonValue)
//        {
//            return col.FindOneById(bsonValue);
//        }

//        /// <summary>
//        /// 根据Id获取第一个
//        /// </summary>
//        /// <param name="documentType">文档类型</param>
//        /// <param name="bsonValue">id</param>
//        /// <returns></returns>
//        public object FindOneById(Type documentType, BsonValue bsonValue)
//        {
//            return col.FindOneByIdAs(documentType, bsonValue);
//        }

//        /// <summary>
//        /// 根据文档名称
//        /// </summary>
//        /// <returns></returns>
//        public string FullName()
//        {
//            return col.FullName;
//        }

//        /// <summary>
//        /// 根据当前的条件或者最近的文档
//        /// </summary>
//        /// <param name="query"></param>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <param name="limit"></param>
//        /// <param name="options"></param>
//        /// <returns></returns>
//        public GeoNearResult<T> GeoNear(IMongoQuery query, double x, double y, int limit, IMongoGeoNearOptions options)
//        {
//            return col.GeoNear(query, x, y, limit, options);
//        }

//        /// <summary>
//        /// 获取当前的集合的状态
//        /// </summary>
//        /// <returns></returns>
//        public CollectionStatsResult GetStats()
//        {
//            return col.GetStats();
//        }

//        /// <summary>
//        /// 获取总的数据大小
//        /// </summary>
//        public long GetTotalDataSize()
//        {
//            return col.GetTotalDataSize();
//        }

//        /// <summary>
//        /// 获取总的存储大小
//        /// </summary>
//        /// <returns></returns>
//        public long GetTotalStorageSize()
//        {
//            return col.GetTotalStorageSize();
//        }

//        /// <summary>
//        /// 验证当前的索引是否存在
//        /// </summary>
//        /// <param name="names"></param>
//        public bool IndexExists(string[] names)
//        {
//            return col.IndexExists(names);
//        }

//        /// <summary>
//        /// 验证当前的索引是否存在
//        /// </summary>
//        /// <param name="names"></param>
//        public bool IndexExistsByName(string name)
//        {
//            return col.IndexExistsByName(name);
//        }

//        /// <summary>
//        /// 插入数据
//        /// </summary>
//        /// <param name="t"></param>
//        /// <param name="options"></param>
//        /// <returns></returns>
//        public WriteConcernResult Insert(T t, MongoInsertOptions options)
//        {
//            return col.Insert(t, options);
//        }

//        /// <summary>
//        /// 批量插入数据
//        /// </summary>
//        /// <param name="lstT"></param>
//        /// <param name="options"></param>
//        /// <returns></returns>
//        public IEnumerable<WriteConcernResult> Insert(IEnumerable<T> lstT, MongoInsertOptions options = null)
//        {
//            if (options == null)
//            {
//                return col.InsertBatch(lstT);
//            }
//            else
//            {
//                return col.InsertBatch(lstT, options);
//            }
//        }

//        /// <summary>
//        /// 标识否允许删除里面的文档数据
//        /// </summary>
//        /// <returns></returns>
//        public bool IsCapped()
//        {
//            return col.IsCapped();
//        }

//        /// <summary>
//        /// 获取集合的名称
//        /// </summary>
//        /// <returns></returns>
//        public string Name()
//        {
//            return col.Name;
//        }

//        /// <summary>
//        /// 删除所有的数据
//        /// </summary>
//        /// <returns></returns>
//        public WriteConcernResult RemoveAll()
//        {
//            return col.RemoveAll();
//        }

//        /// <summary>
//        /// 保存文档
//        /// </summary>
//        /// <param name="document"></param>
//        /// <returns></returns>
//        public WriteConcernResult Save(T document)
//        {
//            return col.Save(document);
//        }

//        /// <summary>
//        /// 验证集合
//        /// </summary>
//        /// <returns></returns>
//        public ValidateCollectionResult Validate()
//        {
//            return col.Validate();
//        }

//        /// <summary>
//        /// 获取当前表的相关信息
//        /// </summary>
//        /// <returns></returns>
//        public MongoCollection<T> GetTableInfo()
//        {
//            //创建数据连接
//            server = new MongoClient(connString).GetServer();
//            //获取指定数据库
//            db = server.GetDatabase(dbName);

//            //获取表
//            col = db.GetCollection<T>(colName);

//            return col;
//        }
//        #endregion
//    }

//    #region 分页
//    /// <summary>
//    /// 分页
//    /// </summary>
//    public class PageInfo
//    {
//        private int pageIndex;
//        /// <summary>
//        /// 当前的页
//        /// </summary>
//        public int PageIndex
//        {
//            get { return pageIndex; }
//            set { pageIndex = value; }
//        }
//        private int pageSize;
//        /// <summary>
//        /// 每页的大小
//        /// </summary>
//        public int PageSize
//        {
//            get { return pageSize; }
//            set { pageSize = value; }
//        }
//        private string[] sort;
//        /// <summary>
//        /// 排序的列名
//        /// </summary>
//        public string[] Sort
//        {
//            get { return sort; }
//            set { sort = value; }
//        }
//        private Direction direction;
//        /// <summary>
//        /// 排序方向
//        /// </summary>
//        public Direction Direction
//        {
//            get { return direction; }
//            set { direction = value; }
//        }
//    }
//    /// <summary>
//    /// 排序的方向
//    /// </summary>
//    public enum Direction
//    {
//        DESC,
//        ASC
//    }
//    #endregion


//}

