using System;
using System.Data;
using ATEC_API.Data.DTO.StagingDTO;
using ATEC_API.Data.IRepositories;
using ATEC_API.Data.StoredProcedures;
using ATEC_API.GeneralModels.MESATECModels.StagingResponse;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ATEC_API.Data.Repositories
{
    public class StagingRepository : IStagingRepository
    {
        private readonly IDapperConnection _dapperConnection;

        public StagingRepository(IDapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        }

        public async Task<IEnumerable<MaterialCustomerResponse>>? GetMaterialCustomer(int paramMaterialType)
        {
            await using SqlConnection sqlConnection = _dapperConnection.MES_ATEC_CreateConnection();

            var CustomerAvailable = await sqlConnection.QueryAsync<MaterialCustomerResponse>(
                                                                            StagingSP.usp_Material_Customer,
                                                                            new
                                                                            {
                                                                                MaterialType = paramMaterialType
                                                                            },
                                                                            commandType: CommandType.StoredProcedure
                                                                            );
            return CustomerAvailable;
        }

        public async Task<IEnumerable<MaterialStagingResponse>>? GetMaterialDetail(MaterialStagingDTO materialStagingDTO)
        {
            await using SqlConnection sqlConnection = _dapperConnection.MES_ATEC_CreateConnection();

            var MaterialDetails = await sqlConnection.QueryAsync<MaterialStagingResponse>(
                                                                            StagingSP.usp_Material_Details,
                                                                            new
                                                                            {
                                                                                SID = materialStagingDTO.Sid,
                                                                                MaterialID = materialStagingDTO.MaterialId,
                                                                                Serial = materialStagingDTO.Serial,
                                                                                ExpirationDate = materialStagingDTO.ExpirationDate,
                                                                                CustomerCode = materialStagingDTO.CustomerCode,
                                                                                MaterialType = materialStagingDTO.MaterialType
                                                                            },
                                                                            commandType: CommandType.StoredProcedure
                                                                            );
            return MaterialDetails;
        }

        public async Task<StagingResponse> IsTrackOut(StagingDTO stagingDTO)
        {
            await using SqlConnection sqlConnection = _dapperConnection.MES_ATEC_CreateConnection();

            var IsTrackOut = await sqlConnection.QueryFirstOrDefaultAsync<StagingResponse>(
                                                                   StagingSP.usp_Staging_IsTrackOut_Test,
                                                                   new
                                                                   {
                                                                       LotAlias = stagingDTO.LotAlias,

                                                                   },
                                                                   commandType: CommandType.StoredProcedure
                                                                   );
            if (IsTrackOut == null)
            {
                return null;
            }

            return IsTrackOut;
        }
    }
}