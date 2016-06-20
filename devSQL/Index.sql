select object_id('IX_Users_Login')

--------------------------------------------
--------------- USERS ----------------------
--------------------------------------------

-- clustered index na User_Id - determinuje uk�ad danych

create unique nonclustered index IX_Users_Login
on inv.Users(Usr_Login asc, Usr_Status, Usr_IsLogged)
with (drop_existing = on)
-- bardzo dobra selektywno��,
-- zawiera kolumny potrzebne do typowych operacji na u�ytkownikach
-- przyspiesza wyszukiwanie usera po loginie

--------------------------------------------
--------------- PARTNERS -------------------
--------------------------------------------

-- clustered index na Part_Id, determinuje uk�ad danych, znacz�co przyspiesza dzia�anie procedury PartnerGetById (Index seek vs Table scan)
-- pozwala na szybsze ��czenie tabel Invoices z Partnerami (Buyer, Vendor)

create unique nonclustered index IX_Partners_Vatin
on inv.Partners(Part_Vatin asc)
with (drop_existing = on)
-- bardzo dobra selektywno��, do wyszukiwania partner po numerze identyfikacji podatkowej

/*create nonclustered index IX_Partners_PartnerData
on inv.Partners(Part_FirstName asc, Part_LastName asc, Part_CompanyName asc)
with (drop_existing = on)*/
-- dobra selektywno��, kolumny pozwalaj� na pokrycie wi�kszo�ci opcji wyszukiwania partnera bez korzystania z Vatinu
-- za du�o danych, nie tworz�
--------------------------------------------
--------------- INVOICES -------------------
--------------------------------------------

-- clustered index na Inv_Id, determinuje uk�ad danych, Przyspiesza ��czenie tabel, Przyspiesza cz�sto wykorzystywan� funkcj� pobierania szczeg�owych danych o fakturze

create nonclustered index IXF_Invoices_New
on inv.Invoices(Inv_Id)
where Inv_Status = 1
with (drop_existing = on)
-- na pocz�tku s�aba selektywno��, kt�ra ro�nie w miar� pracy systemu

create nonclustered index IXF_Invoices_Paid
on inv.Invoices(Inv_Id)
where Inv_Status = 2
with (drop_existing = on)
-- na pocz�tku s�aba selektywno��, kt�ra ro�nie w miar� pracy systemu

create nonclustered index IX_Invoices_Number
on inv.Invoices(Inv_Number)
with (drop_existing = on)
-- bardzo dobra selektywno��, przyspiesza wyszukiwanie faktury po jej numerze