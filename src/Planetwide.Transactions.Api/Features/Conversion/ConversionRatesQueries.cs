using System.Xml.Linq;
using System.Xml.XPath;

namespace Planetwide.Transactions.Api.Features.Conversion;

using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;

public record ConversionRate(string Currency, decimal Rate);

[ExtendObjectType(typeof(QueryRoot))]
public class ConversionRatesQueries
{
    public IEnumerable<ConversionRate> GetConversionRates([Service] IHttpClientFactory factory, [Service] IMemoryCache cache)
    {
        const string ratesCacheKey = "Rates";
        if(cache.TryGetValue(ratesCacheKey, out ConversionRate[] conversionRates))
        {
            return conversionRates;
        }
        
        var rates = XDocument.Load("http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml")
            .XPathSelectElements("//*[@currency]")
            .Select(node =>
            {
                var attributes = node.Attributes()
                    .ToDictionary(x => x.Name, x => x.Value);

                return new ConversionRate(attributes["currency"], decimal.Parse(attributes["rate"]));
            })
            .ToArray();

        cache.Set(ratesCacheKey, rates);
        return rates;
    }
}

