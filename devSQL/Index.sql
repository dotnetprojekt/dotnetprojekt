select object_id('IX_Users_Login')

--------------------------------------------
--------------- USERS ----------------------
--------------------------------------------

-- clustered index na User_Id - determinuje uk³ad danych

create unique nonclustered index IX_Users_Login
on inv.Users(Usr_Login asc, Usr_Status, Usr_IsLogged)
with (drop_existing = on)
-- bardzo dobra selektywnoœæ,
-- zawiera kolumny potrzebne do typowych operacji na u¿ytkownikach
-- przyspiesza wyszukiwanie usera po loginie

--------------------------------------------
--------------- PARTNERS -------------------
--------------------------------------------

-- clustered index na Part_Id, determinuje uk³ad danych, znacz¹co przyspiesza dzia³anie procedury PartnerGetById (Index seek vs Table scan)
-- pozwala na szybsze ³¹czenie tabel Invoices z Partnerami (Buyer, Vendor)

create unique nonclustered index IX_Partners_Vatin
on inv.Partners(Part_Vatin asc)
with (drop_existing = on)
-- bardzo dobra selektywnoœæ, do wyszukiwania partner po numerze identyfikacji podatkowej

/*create nonclustered index IX_Partners_PartnerData
on inv.Partners(Part_FirstName asc, Part_LastName asc, Part_CompanyName asc)
with (drop_existing = on)*/
-- dobra selektywnoœæ, kolumny pozwalaj¹ na pokrycie wiêkszoœci opcji wyszukiwania partnera bez korzystania z Vatinu
-- za du¿o danych, nie tworzê
--------------------------------------------
--------------- INVOICES -------------------
--------------------------------------------

-- clustered index na Inv_Id, determinuje uk³ad danych, Przyspiesza ³¹czenie tabel, Przyspiesza czêsto wykorzystywan¹ funkcjê pobierania szczegó³owych danych o fakturze

create nonclustered index IXF_Invoices_New
on inv.Invoices(Inv_Id)
where Inv_Status = 1
with (drop_existing = on)
-- na pocz¹tku s³aba selektywnoœæ, która roœnie w miarê pracy systemu

create nonclustered index IXF_Invoices_Paid
on inv.Invoices(Inv_Id)
where Inv_Status = 2
with (drop_existing = on)
-- na pocz¹tku s³aba selektywnoœæ, która roœnie w miarê pracy systemu

create nonclustered index IX_Invoices_Number
on inv.Invoices(Inv_Number)
with (drop_existing = on)
-- bardzo dobra selektywnoœæ, przyspiesza wyszukiwanie faktury po jej numerze