using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using MicroServiceBase.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;

namespace MicroServiceBase.BaseImplementations;

public class BaseCRUDController<TEntity, TDTO> : ControllerBase where TEntity : class, IEntity
{
    private readonly IBaseService<TEntity> _baseService;

    private readonly IMapper _mapper;

    private readonly ILogger _logger;

    public BaseCRUDController(IBaseService<TEntity> baseService, IMapper mapper, ILogger logger)
    {
        _baseService = baseService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet(nameof(Get))]
    public virtual async Task<IActionResult> Get(Guid? uuid)
    {
        try
        {
            if (uuid is null)
            {
                IReadOnlyCollection<TEntity> result = await _baseService.GetAsync();
                IEnumerable<TDTO> resultDTO = _mapper.Map<IEnumerable<TDTO>>(result);
                return Ok(resultDTO);
            }
            else
            {
                TEntity result = await _baseService.GetAsync(uuid.Value);
                TDTO resultDTO = _mapper.Map<TDTO>(result);
                return Ok(resultDTO);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
                 
    }

    [HttpPost(nameof(Add))]
    public virtual async Task<IActionResult> Add(TDTO dto)
    {
        try
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            TEntity result = await _baseService.AddAsync(entity);
            TDTO resultDTO = _mapper.Map<TDTO>(result);
            return Ok(resultDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch(nameof(Update))]
    public virtual async Task<IActionResult> Update(TDTO dto)
    {
        try 
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            TEntity result = await _baseService.UpdateAsync(entity);
            TDTO resultDTO = _mapper.Map<TDTO>(result);
            return Ok(resultDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete(nameof(Delete))]
    public virtual async Task<IActionResult> Delete(Guid uuid)
    {
        try
        {
            await _baseService.DeleteAsync(uuid);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost(nameof(Find))]
    public virtual async Task<IActionResult> Find(
        [FromBody] FilterDTO filterDTO,
        [FromQuery] string? orderBy,
        [FromQuery] bool? descending)
    {
        try
        {
            Filter filter = MapFilter(filterDTO);
            IReadOnlyCollection<TEntity> findResult = await _baseService.FindAsync(filter, orderBy, descending);
            IReadOnlyCollection<TDTO> findResultDTO = _mapper.Map<IReadOnlyCollection<TDTO>>(findResult);
            return Ok(findResultDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    private Filter MapFilter(FilterDTO filterDTO)
    { 
        Filter filter = new Filter();
        foreach (FilterConditionDTO filterCondition in filterDTO.Filters)
            filter.Conditions.Add(MapFilterCondition(filterCondition));
        return filter;
    }

    private FilterCondition MapFilterCondition(FilterConditionDTO filterConditionDTO)
        => new FilterCondition()
        {
            Property = filterConditionDTO.Property,
            Condition = Enum.Parse<ComparisonCondition>(filterConditionDTO.Condition),
            Value = GetFindConditionValue(filterConditionDTO.Property, filterConditionDTO.Value)
        };

    private object GetFindConditionValue(string property, string value)
    { 
        Type type = typeof(TEntity);
        if (property.Contains("."))
        {
            string[] properties = property.Split(".");
            foreach (string currentProperty in properties)           
                type = GetPropertyType(type, currentProperty);
        }
        else 
        {
            type = GetPropertyType(type, property);
        }

        TypeConverter typeConvertor = TypeDescriptor.GetConverter(type);
        object? propertyValue = typeConvertor.ConvertFromString(value);
        if (propertyValue is null)
            throw new ArgumentNullException($"Unable to cast value {value} to type {type.Name}");
        return propertyValue;
    }

    private Type GetPropertyType(Type currentType, string property)
    {
        PropertyInfo? propertyInfo = currentType.GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase));

        if (propertyInfo is null)
            throw new ArgumentException($"Type {currentType.Name} doesn't contains property {property}");

        return propertyInfo.PropertyType;
    }
}
