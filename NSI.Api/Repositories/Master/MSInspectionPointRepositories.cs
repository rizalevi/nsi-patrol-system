using ClosedXML;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    public class MSInspectionPointRepositories
    {
        private readonly IMSInspectionPointRepository _repo;

        public MSInspectionPointRepositories(IMSInspectionPointRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetList(string courseCode, string groupCode)
        {
            var data = await _repo.GetList(courseCode, groupCode);
            return data.Select(x => new MSInspectionPointDto
            {
                Course_Code = x.Course_Code,
                Group_Code  = x.Group_Code,
                Inspection_Code = x.Inspection_Code,
                Inspection_Point = x.Inspection_Point,
                Unit_Code = x.Unit_Code,
                Unit_Description = x.Unit_Description,
                Methode_Description = x.Methode_Description,
                Minimum = x.Minimum,
                Min = x.Min,
                Maximum = x.Maximum,
                Max = x.Max,
                Chart = x.Chart,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User,
                Status = x.Status
            });
        }

        #region Combo Fill

        public async Task<IEnumerable<MSInspectionPointDto>> GetClusters()
        {
            return await _repo.GetClusters();
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetPlants(string clusterCode)
        {
            return await _repo.GetPlants(clusterCode);
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetCourses(string plantCode)
        {
            return await _repo.GetCourses(plantCode);
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetGroups(string courseCode)
        {
            return await _repo.GetGroups(courseCode);
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetCharts()
        {
            return await _repo.GetCharts();
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetUnits()
        {
            return await _repo.GetUnits();
        }

        #endregion

        public async Task<IEnumerable<MSInspectionPointDto>> GetAll(string clusterCode)
        {
            var data = await _repo.GetAll(clusterCode);
            return data.Select(x => new MSInspectionPointDto
            {
                Course_Code = x.Course_Code,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update
            });
        }

        public async Task<MSInspectionPointDto> GetData(string courseCode, string groupCode, string inspectionCode)
        {
            var data = await _repo.GetData(courseCode, groupCode, inspectionCode);
            if (data == null) return null;

            return new MSInspectionPointDto
            {
                Course_Code = data.Course_Code,
                Group_Code = data.Group_Code,
                Inspection_Code = data.Inspection_Code,
                Inspection_Point = data.Inspection_Point,
                Unit_Code = data.Unit_Code,
                Unit_Description = data.Unit_Description,
                Methode_Description = data.Methode_Description,
                Minimum = data.Minimum,
                Min = data.Min,
                Maximum = data.Maximum,
                Max = data.Max,
                Option_1 = data.Option_1,
                Option_2 = data.Option_2,
                Chart = data.Chart,
                Register_Date = data.Register_Date,
                Register_User = data.Register_User,
                Last_Update = data.Last_Update,
                Last_User = data.Last_User,
                Status = data.Status
            };
        }

        public async Task<MSInspectionPointDto> CheckInspectionCode(string courseCode, string groupCode, string inspectionCode)
        {
            var data = await _repo.CheckInspectionCode(courseCode, groupCode, inspectionCode);
            if (data == null) return null;

            return new MSInspectionPointDto
            {
                Course_Code = data.Course_Code,
                Group_Code = data.Group_Code,
                Inspection_Code = data.Inspection_Code,
                Inspection_Point = data.Inspection_Point,
                Unit_Code = data.Unit_Code,
                Unit_Description = data.Unit_Description,
                Methode_Description = data.Methode_Description,
                Minimum = data.Min,
                Min = data.Min,
                Maximum = data.Max,
                Max = data.Max,
                Option_1 = data.Option_1,
                Option_2 = data.Option_2,
                Chart = data.Chart,
                Register_Date = data.Register_Date,
                Register_User = data.Register_User,
                Last_Update = data.Last_Update,
                Last_User = data.Last_User,
                Status = data.Status
            };
        }

        public async Task<MSInspectionPointDto> GenerateInspectionCode(string courseCode, string groupCode)
        {
            var data = await _repo.GenerateInspectionCode(courseCode, groupCode);
            if (data == null) return null;

            return new MSInspectionPointDto
            {
                Inspection_Code = data.Inspection_Code
            };
        }

        public async Task<IEnumerable<MSInspectionPointDto>> GetMethode(string unitCode)
        {
            return await _repo.GetMethode(unitCode);
        }

        public async Task Create(MSInspectionPointDto model, string userId)
        {
            var entity = new MSInspectionPointDto
            {
                Course_Code = model.Course_Code,
                Group_Code = model.Group_Code,
                Inspection_Code = model.Inspection_Code,
                Inspection_Point = model.Inspection_Point,
                Unit_Code = model.Unit_Code,
                Unit_Description = model.Unit_Description,
                Methode_Description = model.Methode_Description,
                Minimum = model.Minimum,
                Min = model.Min,
                Maximum = model.Maximum,
                Max = model.Max,
                Chart = model.Chart,
                Register_Date = model.Register_Date,
                Register_User = model.Register_User,
                Last_Update = model.Last_Update,
                Last_User = model.Last_User,
                Status = model.Status,
                Option = model.Option
            };

            await _repo.Create(entity, userId);
        }

        public async Task Update(MSInspectionPointDto model, string userId)
        {
            var entity = new MSInspectionPointDto
            {
                Course_Code = model.Course_Code,
                Group_Code = model.Group_Code,
                Inspection_Code = model.Inspection_Code,
                Inspection_Point = model.Inspection_Point,
                Unit_Code = model.Unit_Code,
                Unit_Description = model.Unit_Description,
                Methode_Description = model.Methode_Description,
                Minimum = model.Minimum,
                Min = model.Min,
                Maximum = model.Maximum,
                Max = model.Max,
                Chart = model.Chart,
                Register_Date = model.Register_Date,
                Register_User = model.Register_User,
                Last_Update = model.Last_Update,
                Last_User = model.Last_User,
                Status = model.Status,
                Option = model.Option
            };

            await _repo.Update(entity, userId);
        }

        public async Task Remove(string courseCode, string groupCode, string inspectionCode, string userID)
        {
            await _repo.Remove(courseCode, groupCode, inspectionCode, userID);
        }
    }
}
