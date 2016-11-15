﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using UclaExt.Common.ExtensionMethods;
using UclaExt.Common.Interfaces;

namespace Talent.DataAccess.Ado
{
    public class HairColorRepository : IRepository<HairColor>
    {
        #region IRepository interface

        public IEnumerable<HairColor> Fetch(object criteria = null)
        {
            var data = new List<HairColor>();
            var connString = ConfigurationManager
                .ConnectionStrings["AppConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (criteria == null)
                    {
                        cmd.CommandText = "select * from HairColor";
                    }
                    else if (criteria is int)
                    {
                        cmd.CommandText = "select * from HairColor where Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", (int)criteria);
                    }
                    else
                    {
                        var msg = String.Format(
                            "HairColorRepository: Unknown criteria type: {0}",
                            criteria);
                        throw new InvalidOperationException(msg);
                    }
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var g = new HairColor();
                        g.Id = dr.AsInt32("Id");
                        g.Code = dr.AsString("Code");
                        g.Name = dr.AsString("Name");
                        g.IsInactive = dr.AsBoolean("IsInactive");
                        g.DisplayOrder = dr.AsInt32("DisplayOrder");

                        data.Add(g);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Saves entity changes to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>updated entity, or null if the entity is deleted</returns>
        public HairColor Persist(HairColor item)
        {
            throw new InvalidOperationException("Cannot persist EyeColor - it is not editable.");
        }

        #endregion

    }
}

