CREATE TABLE DatosComarca (
    Any INT,
    CodiComarca INT,
    Comarca VARCHAR(50),
    Poblacio INT,
    DomesticXarxa INT,
    ActivitatsEconomiques INT,
    Total INT,
    ConsumDomesticPerCapita FLOAT,
    CONSTRAINT PK_DatosComarca PRIMARY KEY (Any, CodiComarca)
);

