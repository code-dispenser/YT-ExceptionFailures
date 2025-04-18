﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using ExceptionFailures.Contracts.Areas.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExceptionFailures.Application.Common.Models.EFCore;

public partial class Supplier
{
    public int    SupplierID    { get; set; }
    public string CompanyName   { get; set; }
    public string ContactName   { get; set; }
    public string ContactTitle  { get; set; }
    public string Address       { get; set; }
    public string City          { get; set; }
    public string Region        { get; set; }
    public string PostalCode    { get; set; }
    public string Country       { get; set; }
    public string Phone         { get; set; }
    public string Fax           { get; set; }
    public string HomePage      { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();



    public static Expression<Func<Supplier, SupplierView>> ProjectToSupplierView
        
        => supplier => new SupplierView(supplier.SupplierID, supplier.CompanyName, supplier.Products.Count());

}