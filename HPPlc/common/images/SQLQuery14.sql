CREATE TABLE CouponCodeEditLog (
  id              INT           NOT NULL    IDENTITY    PRIMARY KEY,
  TransactionId           VARCHAR(100)  NOT NULL,
  CouponCodeId  VARCHAR(100),
  LogDetails Nvarchar(255),
  UserdId int,
  DOC date
);