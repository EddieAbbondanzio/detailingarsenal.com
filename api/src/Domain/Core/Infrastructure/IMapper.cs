namespace DetailingArsenal.Domain {
    public interface IMapper {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}