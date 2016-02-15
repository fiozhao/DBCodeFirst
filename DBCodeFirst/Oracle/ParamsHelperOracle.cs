using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace DBCodeFirst
{
	public class ParamsHelperOracle
	{
		private List<OracleParameter> listParameter = null;

		public List<OracleParameter> ListParameter
		{
			get
			{
				return this.listParameter;
			}
		}

		public ParamsHelperOracle()
		{
			this.listParameter = new List<OracleParameter>();
		}

		public void Add(string parParameterName, object parValue)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parParameterName;
			oracleParameter.Value = parValue;
			this.listParameter.Add(oracleParameter);
		}

		public void Add(string parParameterName, object parValue, DbType parDbType)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parParameterName;
			oracleParameter.Value = parValue;
			oracleParameter.DbType = parDbType;
			this.listParameter.Add(oracleParameter);
		}

		public void Add(string parParameterName, object parValue, DbType parDbType, int parSize)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parParameterName;
			oracleParameter.Value = parValue;
			oracleParameter.DbType = parDbType;
			oracleParameter.Size = parSize;
			this.listParameter.Add(oracleParameter);
		}

		public void Add(string parParameterName, object parValue, DbType parDbType, int parSize, ParameterDirection parDirection)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parParameterName;
			oracleParameter.Value = parValue;
			oracleParameter.DbType = parDbType;
			oracleParameter.Size = parSize;
			oracleParameter.Direction = parDirection;
			this.listParameter.Add(oracleParameter);
		}

		public void Add(string parParameterName, ParameterDirection parDirection)
		{
			OracleParameter oracleParameter = new OracleParameter();
			oracleParameter.ParameterName = parParameterName;
			oracleParameter.Direction = ParameterDirection.Output;
			this.listParameter.Add(oracleParameter);
		}
	}
}
