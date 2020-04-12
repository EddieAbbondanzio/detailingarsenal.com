public class AutoMapperAdapter : IMapper {
    private AutoMapper.IMapper mapper;
    public AutoMapperAdapter(AutoMapper.IMapper mapper) {
        this.mapper = mapper;
    }

    public TDestination Map<TSource, TDestination>(TSource source) {
        return mapper.Map<TSource, TDestination>(source);
    }
}