using FluentMigrator;

namespace DetailingArsenal.Persistence.Admin.ProductCatalog.Migrations {
    [Migration(2021_02_05_00, "Create cascades on foreign keys for pad series.")]
    public class CreateCascadeForeignKeys : Migration {
        public override void Up() {
            // Update pad_images
            Delete.ForeignKey().FromTable("pad_images").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id");
            Delete.ForeignKey().FromTable("pad_images").ForeignColumn("image_id").ToTable("images").PrimaryColumn("id");

            Create.ForeignKey().FromTable("pad_images").ForeignColumn("image_id").ToTable("images").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);
            Create.ForeignKey().FromTable("pad_images").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);

            // Update pad_option_part_numbers
            Delete.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("pad_option_id").ToTable("pad_options").PrimaryColumn("id");
            Delete.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("part_number_id").ToTable("part_numbers").PrimaryColumn("id");

            Create.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("pad_option_id").ToTable("pad_options").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);
            Create.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("part_number_id").ToTable("part_numbers").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);

            // Update pad_options
            Delete.ForeignKey("FK_pad_options_pad_color_id_pad_colors_id").OnTable("pad_options");
            Delete.ForeignKey().FromTable("pad_options").ForeignColumn("pad_size_id").ToTable("pad_sizes").PrimaryColumn("id");

            Create.ForeignKey().FromTable("pad_options").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);
            Create.ForeignKey().FromTable("pad_options").ForeignColumn("pad_size_id").ToTable("pad_sizes").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);

            // Update pads
            Delete.ForeignKey().FromTable("pads").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pads").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);

            // Update pad_series_polisher_types
            Delete.ForeignKey().FromTable("pad_series_polisher_types").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pad_series_polisher_types").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);

            // Update pad_sizes. (Never had a true foreign key. Oops)
            Create.ForeignKey().FromTable("pad_sizes").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id").OnDelete(System.Data.Rule.Cascade);
        }

        public override void Down() {
            Delete.ForeignKey().FromTable("pad_images").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id");
            Delete.ForeignKey().FromTable("pad_images").ForeignColumn("image_id").ToTable("images").PrimaryColumn("id");

            Create.ForeignKey().FromTable("pad_images").ForeignColumn("image_id").ToTable("images").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pad_images").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id");

            Delete.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("pad_option_id").ToTable("pad_options").PrimaryColumn("id");
            Delete.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("part_number_id").ToTable("part_numbers").PrimaryColumn("id");

            Create.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("pad_option_id").ToTable("pad_options").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pad_option_part_numbers").ForeignColumn("part_number_id").ToTable("part_numbers").PrimaryColumn("id");

            Delete.ForeignKey().FromTable("pad_options").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id");
            Delete.ForeignKey().FromTable("pad_options").ForeignColumn("pad_size_id").ToTable("pad_sizes").PrimaryColumn("id");

            Create.ForeignKey("FK_pad_options_pad_color_id_pad_colors_id").FromTable("pad_options").ForeignColumn("pad_id").ToTable("pads").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pad_options").ForeignColumn("pad_size_id").ToTable("pad_sizes").PrimaryColumn("id");

            Delete.ForeignKey().FromTable("pads").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pads").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");

            Delete.ForeignKey().FromTable("pad_series_polisher_types").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");
            Create.ForeignKey().FromTable("pad_series_polisher_types").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");

            Delete.ForeignKey().FromTable("pad_sizes").ForeignColumn("pad_series_id").ToTable("pad_series").PrimaryColumn("id");
        }
    }
}