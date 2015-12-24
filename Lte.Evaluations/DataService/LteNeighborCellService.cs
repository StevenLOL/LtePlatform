﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Evaluations.DataService
{
    public class LteNeighborCellService
    {
        private readonly ILteNeighborCellRepository _repository;

        public LteNeighborCellService(ILteNeighborCellRepository repository)
        {
            _repository = repository;
        }

        public List<LteNeighborCell> QueryCells(int cellId, byte sectorId)
        {
            return _repository.GetAllList(cellId, sectorId);
        }
    }
}
