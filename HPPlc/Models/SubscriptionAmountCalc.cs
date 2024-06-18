using HPPlc.Models.Coupon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPPlc.Models
{
	public class SubscriptionAmountCalc
	{
		public decimal GetSubscriptionAmount()
		{
			decimal subscriptionAmount = 0;

			List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
			UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

			if (UsertempSubscription != null && UsertempSubscription.Any())
			{
				foreach (var subsvald in UsertempSubscription)
				{
					if (!String.IsNullOrEmpty(subsvald.SubscriptionPrice))
					{
						subscriptionAmount += Convert.ToDecimal(subsvald.SubscriptionPrice);
					}
				}
			}

			return subscriptionAmount;
		}
		public decimal GetExistingUserDiscountAmount()
		{
			decimal existingUserDiscountAmount = 0;

			List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
			UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

			if (UsertempSubscription != null && UsertempSubscription.Any())
			{
				foreach (var subsvald in UsertempSubscription)
				{
					existingUserDiscountAmount += Convert.ToDecimal(subsvald.DiscountAmount);
				}
			}

			return existingUserDiscountAmount;
		}

		
		public decimal GetCouponDiscountAmount()
		{
			decimal couponDiscountAmount = 0;
			decimal subscriptionAmount = 0;

			CouponCodeResponse couponCodeResponse = new CouponCodeResponse();
			couponCodeResponse = SessionManagement.GetCurrentSession<CouponCodeResponse>(SessionType.CouponTempDtls);

			List<TempSubscriptionCreatedByUser> UsertempSubscription = new List<TempSubscriptionCreatedByUser>();
			UsertempSubscription = SessionManagement.GetCurrentSession<List<TempSubscriptionCreatedByUser>>(SessionType.SubscriptionTempDtls);

			if ((couponCodeResponse != null && couponCodeResponse.ResponseCode == 1))
			{
				if ((UsertempSubscription != null && UsertempSubscription.Any()) && (couponCodeResponse.IsAppliedForSubscription == 1 || couponCodeResponse.IsCouponAppliedForAgeGroup == 1))
				{
					int discountRest = 0;
					foreach (var subsvald in UsertempSubscription)
					{
						if (!String.IsNullOrEmpty(subsvald.SubscriptionPrice) && Convert.ToDecimal(subsvald.SubscriptionPrice) > 0 && couponCodeResponse.DiscountValue > 0)
						{
							//Subscription Discount
							if (couponCodeResponse != null && couponCodeResponse.ResponseCode == 1 && couponCodeResponse.IsAppliedForSubscription == 1)
							{
								string[] subscriptnString = couponCodeResponse.SubscriptionRanking.Split(',');
								if (subscriptnString.Length > 0)
								{
									foreach (var couponitem in subscriptnString.GroupBy(x => x).ToList())
									{
										//var CntCouponAmt = UsertempSubscription?.GroupBy(x => new { x.Ranking }).Any(g => g.Count() > 1);
										if (couponitem.Key == subsvald.Ranking && couponCodeResponse.BenefitRestrict == 1 && discountRest == 0)
										{
											if (couponCodeResponse.DiscountType == 1) // percent discount
											{
												couponDiscountAmount += ((Convert.ToDecimal(subsvald.SubscriptionPrice) * couponCodeResponse.DiscountValue) / 100);
											}
											else if (couponCodeResponse.DiscountType == 2) // flat discount
											{
												couponDiscountAmount += couponCodeResponse.DiscountValue;
											}

											discountRest++;
										}
										else if (couponitem.Key == subsvald.Ranking && couponCodeResponse.BenefitRestrict == 0)
										{
											if (couponCodeResponse.DiscountType == 1) // percent discount
											{
												couponDiscountAmount += ((Convert.ToDecimal(subsvald.SubscriptionPrice) * couponCodeResponse.DiscountValue) / 100);
											}
											else if (couponCodeResponse.DiscountType == 2) // flat discount
											{
												couponDiscountAmount += couponCodeResponse.DiscountValue;
											}
										}
									}
								}
								//Age group coupon discount
								else if (couponCodeResponse != null && couponCodeResponse.ResponseCode == 1 && couponCodeResponse.IsCouponAppliedForAgeGroup == 1)
								{
									string[] agegroupString = couponCodeResponse.AgeGroupAppliedForCoupon.Split(',');
									if (agegroupString.Length > 0)
									{
										foreach (var couponitem in agegroupString.GroupBy(x => x).ToList())
										{
											if (couponitem.Key == subsvald.AgeGroup)
											{
												if (couponCodeResponse.DiscountType == 1) // percent discount
												{
													couponDiscountAmount += ((Convert.ToDecimal(subsvald.SubscriptionPrice) * couponCodeResponse.DiscountValue) / 100);
												}
												else if (couponCodeResponse.DiscountType == 2) // flat discount
												{
													couponDiscountAmount += couponCodeResponse.DiscountValue;
												}

											}
										}
									}
								}

							}
						}
					}
				}
				else
				{
					if (UsertempSubscription != null)
					{
						foreach (var subsvald in UsertempSubscription)
						{

							if (!String.IsNullOrEmpty(subsvald.SubscriptionPrice) && Convert.ToDecimal(subsvald.SubscriptionPrice) > 0 && couponCodeResponse.DiscountValue > 0)
							{
								subscriptionAmount += Convert.ToDecimal(subsvald.SubscriptionPrice) - Convert.ToDecimal(subsvald.DiscountAmount);
							}

						}
					}

					if (subscriptionAmount > 0)
					{
						if (couponCodeResponse.DiscountType == 1) // percent discount
						{
							couponDiscountAmount += ((subscriptionAmount * couponCodeResponse.DiscountValue) / 100);
						}
						else if (couponCodeResponse.DiscountType == 2) // flat discount
						{
							couponDiscountAmount += couponCodeResponse.DiscountValue;
						}
					}
				}
			}

			return couponDiscountAmount;
		}

		public decimal GetPayableAmount()
			{
				decimal payableAmount = 0;
				decimal subscriptionAmount = 0;
				decimal existingUserDiscountAmount = 0;
				decimal couponDiscountAmount = 0;

				subscriptionAmount = GetSubscriptionAmount();
				existingUserDiscountAmount = GetExistingUserDiscountAmount();
				couponDiscountAmount = GetCouponDiscountAmount();

				payableAmount = subscriptionAmount - existingUserDiscountAmount - couponDiscountAmount;

				if (payableAmount <= 0)
					payableAmount = 0;

				return Math.Floor(payableAmount);
			}
		}
	}