USE itaccept_teste;

DROP TABLE IF EXISTS `embarcadoras_transportadoras`;
DROP TABLE IF EXISTS `lances`;
DROP TABLE IF EXISTS `ofertas`;

DROP TABLE IF EXISTS `usuarios`;
DROP TABLE IF EXISTS `produtos`;
DROP TABLE IF EXISTS `empresas`;

CREATE TABLE `empresas` (
	empresa_id INT AUTO_INCREMENT,
    nome_empresa VARCHAR(300) NOT NULL,
    status BIT NOT NULL,
    tipo_empresa VARCHAR(50) NOT NULL,
    PRIMARY KEY(empresa_id)
);

CREATE TABLE `usuarios` (
	usuario_id INT AUTO_INCREMENT,
    nome_usuario VARCHAR(300) NOT NULL,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(1000) NOT NULL,
    password_hash VARBINARY(4000) NOT NULL,
    empresa_id INT NULL,
    status BIT NOT NULL,
    tipo_usuario VARCHAR(50) NOT NULL,
    PRIMARY KEY(usuario_id),
    FOREIGN KEY(empresa_id) REFERENCES empresas(empresa_id)
);

CREATE TABLE `produtos` (
	produto_id INT AUTO_INCREMENT,
    nome_produto VARCHAR(300) NOT NULL,
    embarcadora_id INT NOT NULL,
    status BIT NOT NULL,
    PRIMARY KEY(produto_id),
    FOREIGN KEY(embarcadora_id) REFERENCES empresas(empresa_id)
);

CREATE TABLE `embarcadoras_transportadoras` (
	embarcadora_transportadora_id INT AUTO_INCREMENT,
    embarcadora_id INT NOT NULL,
    transportadora_id INT NOT NULL,
    PRIMARY KEY(embarcadora_transportadora_id),
    FOREIGN KEY(embarcadora_id) REFERENCES empresas(empresa_id),
    FOREIGN KEY(transportadora_id) REFERENCES empresas(empresa_id)
);

CREATE TABLE `ofertas` (
	oferta_id INT AUTO_INCREMENT,
    embarcadora_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade DECIMAL(10,2) NOT NULL,
    endereco_origem VARCHAR(500) NOT NULL,
    endereco_destino VARCHAR(500) NOT NULL,    
    status BIT NOT NULL,
    PRIMARY KEY(oferta_id),
    FOREIGN KEY(embarcadora_id) REFERENCES empresas(empresa_id),
    FOREIGN KEY(produto_id) REFERENCES produtos(produto_id)
);

CREATE TABLE `lances` (
	lance_id INT AUTO_INCREMENT,
    oferta_id INT NOT NULL,
    transportadora_id INT NOT NULL,
    volume DECIMAL(10, 2) NOT NULL,
    preco DECIMAL(10, 2) NOT NULL,
    lance_vencedor BIT NOT NULL,
    status BIT NOT NULL,
    PRIMARY KEY(lance_id),
    FOREIGN KEY(oferta_id) REFERENCES ofertas(oferta_id),
    FOREIGN KEY(transportadora_id) REFERENCES empresas(empresa_id)
);


LOCK TABLES `empresas` WRITE;
INSERT INTO `empresas` VALUES (1,'Embarcadora 01',_binary '','Embarcadora'),(2,'Transportadora 01',_binary '','Transportadora'),(3,'Transportadora 02',_binary '','Transportadora'),(4,'Embarcadora 02',_binary '','Embarcadora'),(5,'Transportadora 03',_binary '','Transportadora');
UNLOCK TABLES;


LOCK TABLES `embarcadoras_transportadoras` WRITE;
INSERT INTO `embarcadoras_transportadoras` VALUES (2,1,2),(3,1,5),(4,4,3),(5,4,5);
UNLOCK TABLES;

LOCK TABLES `produtos` WRITE;
INSERT INTO `produtos` VALUES (1,'E01. Produto 01',1,_binary ''),(2,'E01. Produto 02',1,_binary ''),(3,'E01. Produto 03',1,_binary ''),(4,'E02. Produto 01',4,_binary ''),(5,'E02. Produto 02',4,_binary '');
UNLOCK TABLES;

LOCK TABLES `ofertas` WRITE;
INSERT INTO `ofertas` VALUES (1,1,1,12.00,'Endereço 1','Endereço 2',_binary ''),(2,1,2,20.00,'Endereço Origem','Endereço Destino',_binary ''),(3,1,3,15.00,'Endereço Origem','Endereço Destino',_binary ''),(4,4,4,10.00,'Endereço Origem','Endereço Destino',_binary ''),(5,4,5,50.00,'Endereço Origem','Endereço Destino',_binary '');
UNLOCK TABLES;

LOCK TABLES `lances` WRITE;
INSERT INTO `lances` VALUES (1,1,5,12.00,400.00,_binary '',_binary ''),(2,1,2,12.00,350.00,_binary '\0',_binary ''),(3,2,2,10.00,500.00,_binary '',_binary ''),(4,2,5,10.00,100.00,_binary '',_binary '');
UNLOCK TABLES;

LOCK TABLES `usuarios` WRITE;
INSERT INTO `usuarios` VALUES (1,'Admin','admin','v6X18nMH+mNrsisWUvkH6lUOkvxAjfAd2Np//JfB8ic=',_binary '\إ«YP²o$\霬',NULL,_binary '','Admin'),(3,'E01 - User 01','e01.user01','kN7fHM1sa271HB0LmBwEkKh0Z5UvI50GPu6g2+p7iKs=',_binary '&:hvY\Üؘ[±Ӓ=(',1,_binary '','Funcionario'),(4,'E02 - User 01','e02.user01','8QuHDJdShGdWjeTOpj0rMUNfOx9AOE8ZqTzsQbPy99U=',_binary 'T^]|\М鮼#\Ȯ\Ȳ>',4,_binary '','Funcionario'),(5,'T01 - User 01','t01.user01','1VpiXU1icF3rEKgmm2Lpk6nAj6G3VMJqxApxvbyF2GI=',_binary '\�¡u»¾\ՅzC¥J',2,_binary '','Funcionario'),(6,'T02 - User01','t02.user01','NWl6ydkn/lpj7V6PlZjNThJqZIqirfAdRUfyaPfIICs=',_binary 'w\⥇u\ʝ©¢剌ׄ',3,_binary '','Funcionario'),(7,'T 03 - User 01','t03.user01','zSGIeUWF0J/9fqv481EqmOicArfFKauTlAdk/paXISg=',_binary '\ꘆp𜠒¿\ں¯ª\',5,_binary '','Funcionario');
UNLOCK TABLES;
