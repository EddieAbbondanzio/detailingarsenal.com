using System;
using System.Collections.Generic;
using System.Text;

namespace DetailingArsenal.Persistence.ProductCatalog {
    public class PadsViewQueryBuilder : IService {
        StringBuilder sb = new("select * from pads_view");
        bool addedParams = false;

        public void AddBrandFilter() {
            And();
            sb.Append("brands = any(@Brands)");
        }

        public void AddSeriesFilter() {
            And();
            sb.Append("series = any(@Series)");
        }

        public void AddCategoriesFilter() {
            And();
            sb.Append("category & @Categories > 0");
        }

        public void AddMaterialsFilter() {
            And();
            sb.Append("material = any(@Materials)");
        }

        public void AddTexturesFilter() {
            And();
            sb.Append("material = any(@Textures)");
        }

        public void AddPolisherTypesFilter() {
            And();
            sb.Append("material & @PolisherTypes > 0");
        }

        public void AddHasCenterHoleFilter() {
            And();
            sb.Append("has_center_hole = any(@HasCenterHole)");
        }

        public void AddStarsFilter() {
            And();
            sb.Append("cast(stars as int) = any(@Stars)");
        }

        public override string ToString() => sb.ToString();

        void And() {
            if (!addedParams) {
                sb.Append(" where ");
                addedParams = true;
            } else {
                sb.Append(" and ");
            }
        }
    }
}