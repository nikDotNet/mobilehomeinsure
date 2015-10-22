﻿using MobileHome.Insure.Model;
using MobileHome.Insure.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHome.Insure.Service.Master
{
    public interface IMasterServiceFacade
    {
        #region State Operations
        List<State> GetStates();
        #endregion

        #region Pack Operations
        List<Park> GetParks();

        List<Park> GetParksWithOnOff();

        List<ParkDto> GetListPark();

        Park GetParkById(int id);

        List<Park> FindParkByZip(int zip);

        void SavePark(Park parkObj, bool toDelete = false);

        void SavePark(List<Park> importParks);

        bool OnOrOffPark(int id, bool isOff = false);

        List<OrderDto> GetListOrder();

        List<Customer> GetListCustomers(string zipCode, string lastName);

        List<OrderDto> GetListPremiums(int stateId, string zipCode, DateTime? startDate, DateTime? endDate);
        #endregion
    }
}
