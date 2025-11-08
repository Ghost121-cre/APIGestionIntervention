SELECT 
    'Utilisateurs' as NomTable, 
    IDENT_CURRENT('Utilisateurs') as ValeurIdentity
UNION ALL
SELECT 'Clients', IDENT_CURRENT('Clients')
UNION ALL  
SELECT 'Produits', IDENT_CURRENT('Produits')
UNION ALL
SELECT 'Incidents', IDENT_CURRENT('Incidents')
UNION ALL
SELECT 'Interventions', IDENT_CURRENT('Interventions');