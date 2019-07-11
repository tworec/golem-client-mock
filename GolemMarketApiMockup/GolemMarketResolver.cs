﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GolemMarketApiMockup
{

    [StructLayout(LayoutKind.Sequential)]
    struct StringRef
    {
        public IntPtr Bytes;
        public uint Length;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct StringRefArray
    {
        public StringRef[] Refs;
        public uint Length;
    }

    public class GolemMarketResolver
    {

        public enum ResultEnum
        {
            Error = -1,
            True = 1, 
            False = 0,
            Undefined = 2
        }

        [DllImport("market_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 match_demand_offer(StringRef[] demand_props, uint demand_props_count,
                                                       StringRef demand_constraints,
                                                       StringRef[] offer_props, uint offer_props_count,
                                                       StringRef offer_constraints);

        [DllImport("market_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 resolve_expression(StringRef expression, StringRef[] props, uint props_count);

        public ResultEnum MatchDemandOffer(String[] demand_props, String demand_constraints,
                                               String[] offer_props, String offer_constraints)
        {
            var demand_props_packed = PackStringArray(demand_props);
            var offer_props_packed = PackStringArray(offer_props);
            var demand_constraints_packed = PackString(demand_constraints);
            var offer_constraints_packed = PackString(offer_constraints);

            try
            {
                return (ResultEnum) match_demand_offer(demand_props_packed.Refs, demand_props_packed.Length,
                                          demand_constraints_packed,
                                          offer_props_packed.Refs, offer_props_packed.Length, 
                                          offer_constraints_packed);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return ResultEnum.Error;
            }
            finally
            {
                foreach (var pointer in demand_props_packed.Refs.Select(prop => prop.Bytes)
                    .Concat(offer_props_packed.Refs.Select(prop => prop.Bytes))
                    .Append(demand_constraints_packed.Bytes)
                    .Append(offer_constraints_packed.Bytes))
                {
                    Marshal.FreeHGlobal(pointer);
                }
            }
        }

        public ResultEnum ResolveExpression(String expression, String[] props)
        {
            var props_packed = PackStringArray(props);
            var expression_packed = PackString(expression);

            try
            {
                return (ResultEnum) resolve_expression(expression_packed, props_packed.Refs, props_packed.Length);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                return ResultEnum.Error;
            }
        }


        private StringRefArray PackStringArray(String[] strs)
        {
            var refs = new StringRef[strs.Length];
            var handles = new IntPtr[strs.Length];

            for (int i=0; i<refs.Length; i++)
            {
                refs[i] = PackString(strs[i]);
            }

            return new StringRefArray()
            {
                Refs = refs,
                Length = (uint)strs.Length
            };
        }

        private StringRef PackString(String str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var pointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, pointer, bytes.Length);
            return new StringRef
            {
                Bytes = pointer,
                Length = (uint)bytes.Length
            };
        }

        private byte[] PackStringCStyle(String str)
        {
            return Encoding.UTF8.GetBytes(str+"\0");
        }

    }
}
