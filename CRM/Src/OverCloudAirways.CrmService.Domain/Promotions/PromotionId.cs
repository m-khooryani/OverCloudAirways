﻿using OverCloudAirways.BuildingBlocks.Domain.Models;

namespace OverCloudAirways.CrmService.Domain.Promotions;

public class PromotionId : TypedId<Guid>
{
    public PromotionId(Guid value) : base(value)
    {
    }

    public static PromotionId New()
    {
        return new PromotionId(Guid.NewGuid());
    }
}
