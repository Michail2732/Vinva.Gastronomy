

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;
COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 223 (class 1259 OID 16396)
-- Name: Categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Categories" (
    "Id" uuid NOT NULL,
    "Type" integer NOT NULL,
    "Name" character varying(64) NOT NULL,
    "Description" character varying(256) NOT NULL,
    "Comment" character varying(512),
    "IsDeleted" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Categories" OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 74803)
-- Name: Images; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Images" (
    "Id" uuid NOT NULL,
    "Name" character varying(64) NOT NULL,
    "Format" character varying(64) NOT NULL,
    "ContentType" text NOT NULL,
    "OwnerId" text NOT NULL,
    "Size" bigint NOT NULL,
    "UploadedAt" timestamp with time zone NOT NULL,
    "Group" character varying(128)
);


ALTER TABLE public."Images" OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 16470)
-- Name: IngredientCategories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."IngredientCategories" (
    "CategoriesId" uuid NOT NULL,
    "IngredientId" uuid NOT NULL
);


ALTER TABLE public."IngredientCategories" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 16422)
-- Name: Ingredients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Ingredients" (
    "Id" uuid NOT NULL,
    "UsageComment" character varying(512),
    "PhotoId" uuid,
    "RecipeId" uuid,
    "Name" character varying(64) NOT NULL,
    "Description" character varying(256) NOT NULL,
    "Comment" character varying(512),
    "IsDeleted" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Ingredients" OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 16437)
-- Name: RecipeCategories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RecipeCategories" (
    "CategoriesId" uuid NOT NULL,
    "RecipeId" uuid NOT NULL
);


ALTER TABLE public."RecipeCategories" OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 16487)
-- Name: RecipeIngredients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RecipeIngredients" (
    "RecipeId" uuid NOT NULL,
    "IngredientId" uuid NOT NULL,
    "IsRequired" boolean NOT NULL,
    "Name" character varying(64) NOT NULL,
    "Description" character varying(256) NOT NULL,
    "Comment" character varying(512),
    "Quantities" character varying(128) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE public."RecipeIngredients" OWNER TO postgres;

--
-- TOC entry 4998 (class 0 OID 0)
-- Dependencies: 229
-- Name: COLUMN "RecipeIngredients"."Quantities"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public."RecipeIngredients"."Quantities" IS 'Format: quantity1:unit1;quantity2:unit2;...';


--
-- TOC entry 227 (class 1259 OID 16454)
-- Name: RecipeSteps; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RecipeSteps" (
    "RecipeId" uuid NOT NULL,
    "SeqNumber" integer NOT NULL,
    "PhotoId" uuid,
    "Description" character varying(256) NOT NULL,
    "Comment" character varying(512)
);


ALTER TABLE public."RecipeSteps" OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 16407)
-- Name: Recipes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Recipes" (
    "Id" uuid NOT NULL,
    "BaseRecipe" uuid,
    "PhotoId" uuid,
    "VideoId" uuid,
    "CookingTime" interval,
    "CookingComment" character varying(512),
    "IngredientComment" character varying(512),
    "StorageComment" character varying(512),
    "UsageComment" character varying(512),
    "Name" character varying(64) NOT NULL,
    "Description" character varying(256) NOT NULL,
    "Comment" character varying(512),
    "IsDeleted" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Recipes" OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 74817)
-- Name: RegistrationTokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."RegistrationTokens" (
    "Id" uuid NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "PasswordHash" text NOT NULL,
    "Login" text NOT NULL,
    "Email" text NOT NULL,
    "LetterSentTimestap" timestamp with time zone,
    "ConfirmCompleateTimestap" timestamp with time zone
);


ALTER TABLE public."RegistrationTokens" OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 54100)
-- Name: UserTokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserTokens" (
    "UserId" uuid NOT NULL,
    "AccessToken" text,
    "RefreshToken" text
);


ALTER TABLE public."UserTokens" OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 54087)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Login" character varying(100) NOT NULL,
    "PasswordHash" text NOT NULL,
    "Email" character varying(100) NOT NULL,
    "State" integer NOT NULL,
    "LastLoginAt" timestamp with time zone,
    "LastLogoutAt" timestamp with time zone,
    "LastPasswordChangedAt" timestamp with time zone,
    "LastEmailChangedAt" timestamp with time zone,
    "Roles" text DEFAULT ''::text NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16389)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) CONSTRAINT "_EFMigrationsHistory_MigrationId_not_null" NOT NULL,
    "ProductVersion" character varying(32) CONSTRAINT "_EFMigrationsHistory_ProductVersion_not_null" NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;


--
-- TOC entry 4989 (class 0 OID 54100)
-- Dependencies: 231
-- Data for Name: UserTokens; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."UserTokens" ("UserId", "AccessToken", "RefreshToken") FROM stdin;
019c9982-f11c-7168-ba73-04bf6742691d	eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwMTljOTk4Mi1mMTFjLTcxNjgtYmE3My0wNGJmNjc0MjY5MWQiLCJ1bmlxdWVfbmFtZSI6IlF3ZXJ0eSIsIlN0YXRlIjoiQWN0aXZlIiwiZW1haWwiOiJtaWhhaWxlcmljaGV2QGdtYWlsLmNvbSIsInJvbGUiOlsiQ2xpZW50IiwiTWFuYWdlciJdLCJqdGkiOiIwMTlkOWY3NC03NjhjLTczZjktOWMzZS0xYWIxMjYzOWRjOWUiLCJpYXQiOjE3NzY0OTY3MDMsIm5iZiI6MTc3NjQ5NjcwMywiZXhwIjoxNzc2NTAwMzAzLCJpc3MiOiJHYXN0cm9ub215MTYyMSIsImF1ZCI6Ikdhc3Ryb25vbXkyNzMyIn0.h3X0EseENnhNP6Kcm9is1mdceuGlLTcy9KAf9eboeY0	019d9f74768d75d998cf8ac190db4f3a019d9f74768d75da8200477eef77e168
\.


--
-- TOC entry 4988 (class 0 OID 54087)
-- Dependencies: 230
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Login", "PasswordHash", "Email", "State", "LastLoginAt", "LastLogoutAt", "LastPasswordChangedAt", "LastEmailChangedAt", "Roles") FROM stdin;
019c9982-f11c-7168-ba73-04bf6742691d	Qwerty	$2a$12$eddjpD.q3xfzHMEaBU2pq.eRHFfiUKE5co6vRfjQF0Fm9bbLmSNMS	mihailerichev@gmail.com	0	2026-04-18 10:18:23.117109+03	2026-04-02 10:25:19.20785+03	\N	\N	Client,Manager
\.


--
-- TOC entry 4980 (class 0 OID 16389)
-- Dependencies: 222
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20251123142011_InitialCreate	9.0.10
20260210191425_AddIndexForNameColumn	9.0.10
20260212105641_ChangeMeasureToQuantities	9.0.10
20260216091125_change_unique_name_constraint	9.0.10
20260216105322_change_recipe_step	9.0.10
20260216112023_change_base_recipe	9.0.10
20251123143616_InitialCreate	9.0.10
20260226083733_InitialIdentity	9.0.10
20260302084458_softDelete	9.0.10
20260316144139_ChangeTypeOfRolesProperty	9.0.10
20260316143448_Init	9.0.10
20260330101855_CreateRegistrationTokensTable	9.0.10
20260331141400_DeleteExpiresProp	9.0.10
20260331142018_RemoveNullContraintForTokens	9.0.10
\.


--
-- TOC entry 4791 (class 2606 OID 16406)
-- Name: Categories PK_Categories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");


--
-- TOC entry 4820 (class 2606 OID 74816)
-- Name: Images PK_Images; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "PK_Images" PRIMARY KEY ("Id");


--
-- TOC entry 4807 (class 2606 OID 16476)
-- Name: IngredientCategories PK_IngredientCategories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."IngredientCategories"
    ADD CONSTRAINT "PK_IngredientCategories" PRIMARY KEY ("CategoriesId", "IngredientId");


--
-- TOC entry 4799 (class 2606 OID 16431)
-- Name: Ingredients PK_Ingredients; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ingredients"
    ADD CONSTRAINT "PK_Ingredients" PRIMARY KEY ("Id");


--
-- TOC entry 4802 (class 2606 OID 16443)
-- Name: RecipeCategories PK_RecipeCategories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeCategories"
    ADD CONSTRAINT "PK_RecipeCategories" PRIMARY KEY ("CategoriesId", "RecipeId");


--
-- TOC entry 4810 (class 2606 OID 16499)
-- Name: RecipeIngredients PK_RecipeIngredients; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeIngredients"
    ADD CONSTRAINT "PK_RecipeIngredients" PRIMARY KEY ("RecipeId", "IngredientId");


--
-- TOC entry 4804 (class 2606 OID 16464)
-- Name: RecipeSteps PK_RecipeSteps; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeSteps"
    ADD CONSTRAINT "PK_RecipeSteps" PRIMARY KEY ("RecipeId", "SeqNumber");


--
-- TOC entry 4795 (class 2606 OID 16416)
-- Name: Recipes PK_Recipes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Recipes"
    ADD CONSTRAINT "PK_Recipes" PRIMARY KEY ("Id");


--
-- TOC entry 4822 (class 2606 OID 74828)
-- Name: RegistrationTokens PK_RegistrationTokens; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RegistrationTokens"
    ADD CONSTRAINT "PK_RegistrationTokens" PRIMARY KEY ("Id");


--
-- TOC entry 4818 (class 2606 OID 54110)
-- Name: UserTokens PK_UserTokens; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserTokens"
    ADD CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId");


--
-- TOC entry 4814 (class 2606 OID 54099)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 4788 (class 2606 OID 16395)
-- Name: __EFMigrationsHistory PK__EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK__EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4789 (class 1259 OID 17586)
-- Name: IX_Categories_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Categories_Name" ON public."Categories" USING btree ("Name");


--
-- TOC entry 4805 (class 1259 OID 16510)
-- Name: IX_IngredientCategories_IngredientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_IngredientCategories_IngredientId" ON public."IngredientCategories" USING btree ("IngredientId");


--
-- TOC entry 4796 (class 1259 OID 17585)
-- Name: IX_Ingredients_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Ingredients_Name" ON public."Ingredients" USING btree ("Name");


--
-- TOC entry 4797 (class 1259 OID 16511)
-- Name: IX_Ingredients_RecipeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Ingredients_RecipeId" ON public."Ingredients" USING btree ("RecipeId");


--
-- TOC entry 4800 (class 1259 OID 16512)
-- Name: IX_RecipeCategories_RecipeId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_RecipeCategories_RecipeId" ON public."RecipeCategories" USING btree ("RecipeId");


--
-- TOC entry 4808 (class 1259 OID 16513)
-- Name: IX_RecipeIngredients_IngredientId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_RecipeIngredients_IngredientId" ON public."RecipeIngredients" USING btree ("IngredientId");


--
-- TOC entry 4792 (class 1259 OID 49345)
-- Name: IX_Recipes_BaseRecipe; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Recipes_BaseRecipe" ON public."Recipes" USING btree ("BaseRecipe");


--
-- TOC entry 4793 (class 1259 OID 17583)
-- Name: IX_Recipes_Name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Recipes_Name" ON public."Recipes" USING btree ("Name");


--
-- TOC entry 4815 (class 1259 OID 54117)
-- Name: IX_UserTokens_AccessToken; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserTokens_AccessToken" ON public."UserTokens" USING btree ("AccessToken");


--
-- TOC entry 4816 (class 1259 OID 54118)
-- Name: IX_UserTokens_RefreshToken; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_UserTokens_RefreshToken" ON public."UserTokens" USING btree ("RefreshToken");


--
-- TOC entry 4811 (class 1259 OID 54119)
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- TOC entry 4812 (class 1259 OID 54116)
-- Name: IX_Users_Login; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Login" ON public."Users" USING btree ("Login");


--
-- TOC entry 4828 (class 2606 OID 16477)
-- Name: IngredientCategories FK_IngredientCategories_Categories_CategoriesId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."IngredientCategories"
    ADD CONSTRAINT "FK_IngredientCategories_Categories_CategoriesId" FOREIGN KEY ("CategoriesId") REFERENCES public."Categories"("Id") ON DELETE CASCADE;


--
-- TOC entry 4829 (class 2606 OID 16482)
-- Name: IngredientCategories FK_IngredientCategories_Ingredients_IngredientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."IngredientCategories"
    ADD CONSTRAINT "FK_IngredientCategories_Ingredients_IngredientId" FOREIGN KEY ("IngredientId") REFERENCES public."Ingredients"("Id") ON DELETE CASCADE;


--
-- TOC entry 4824 (class 2606 OID 16432)
-- Name: Ingredients FK_Ingredients_Recipes_RecipeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Ingredients"
    ADD CONSTRAINT "FK_Ingredients_Recipes_RecipeId" FOREIGN KEY ("RecipeId") REFERENCES public."Recipes"("Id") ON DELETE CASCADE;


--
-- TOC entry 4825 (class 2606 OID 16444)
-- Name: RecipeCategories FK_RecipeCategories_Categories_CategoriesId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeCategories"
    ADD CONSTRAINT "FK_RecipeCategories_Categories_CategoriesId" FOREIGN KEY ("CategoriesId") REFERENCES public."Categories"("Id") ON DELETE CASCADE;


--
-- TOC entry 4826 (class 2606 OID 16449)
-- Name: RecipeCategories FK_RecipeCategories_Recipes_RecipeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeCategories"
    ADD CONSTRAINT "FK_RecipeCategories_Recipes_RecipeId" FOREIGN KEY ("RecipeId") REFERENCES public."Recipes"("Id") ON DELETE CASCADE;


--
-- TOC entry 4830 (class 2606 OID 16500)
-- Name: RecipeIngredients FK_RecipeIngredients_Ingredients_IngredientId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeIngredients"
    ADD CONSTRAINT "FK_RecipeIngredients_Ingredients_IngredientId" FOREIGN KEY ("IngredientId") REFERENCES public."Ingredients"("Id") ON DELETE CASCADE;


--
-- TOC entry 4831 (class 2606 OID 16505)
-- Name: RecipeIngredients FK_RecipeIngredients_Recipes_RecipeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeIngredients"
    ADD CONSTRAINT "FK_RecipeIngredients_Recipes_RecipeId" FOREIGN KEY ("RecipeId") REFERENCES public."Recipes"("Id") ON DELETE CASCADE;


--
-- TOC entry 4827 (class 2606 OID 16465)
-- Name: RecipeSteps FK_RecipeSteps_Recipes_RecipeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."RecipeSteps"
    ADD CONSTRAINT "FK_RecipeSteps_Recipes_RecipeId" FOREIGN KEY ("RecipeId") REFERENCES public."Recipes"("Id") ON DELETE CASCADE;


--
-- TOC entry 4823 (class 2606 OID 49346)
-- Name: Recipes FK_Recipes_Recipes_BaseRecipe; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Recipes"
    ADD CONSTRAINT "FK_Recipes_Recipes_BaseRecipe" FOREIGN KEY ("BaseRecipe") REFERENCES public."Recipes"("Id") ON DELETE SET NULL;


--
-- TOC entry 4832 (class 2606 OID 54111)
-- Name: UserTokens FK_UserTokens_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserTokens"
    ADD CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id");




INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('71b5608c-304b-58f9-b5a8-51d8c4bfcb02',NULL,NULL,NULL,'Агар-агар','Агар-агар','Раздел: Для глазури',false),
	 ('2b49d103-8220-5a4b-8c67-7c4d5d07bf80',NULL,NULL,NULL,'Аквафаба','Аквафаба','дегидрированная; Раздел: 3 вариант',false),
	 ('4920840b-dc84-5fa9-a148-9d74e8ff234a',NULL,NULL,NULL,'Ананас','Ананас','Раздел: Ингредиенты для карри',false),
	 ('1979006a-d7d7-505f-a029-4dea97dff18c',NULL,NULL,NULL,'Апельсиновый сок','Апельсиновый сок','Раздел: Барбекю соус',false),
	 ('404a9c1f-8fba-5ee7-b828-40256eb53aab',NULL,NULL,NULL,'Базилик сушеный','Базилик сушеный',NULL,false),
	 ('0020c500-2165-5b16-aba8-775ab26990bd',NULL,NULL,NULL,'Баклажан','Баклажан','Раздел: Ингредиенты для «бекона»',false),
	 ('19a55aa8-5c1e-51fd-94f0-f69d724dad2e',NULL,NULL,NULL,'Банан','Банан','Раздел: Ингредиенты на 6 кексов',false),
	 ('c1e42c12-b4a4-5baf-a322-18dba72b4eeb',NULL,NULL,NULL,'Батат запеченный','Батат запеченный',NULL,false),
	 ('212eb083-c0ee-56fa-a173-762130cdb096',NULL,NULL,NULL,'Блины "шоколадные"','Блины "шоколадные"',NULL,false),
	 ('54b71c87-9feb-5500-8376-71646a51c747',NULL,NULL,NULL,'Блины','Блины',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('be0e5010-6f3d-5b34-8686-3b95354f32cf',NULL,NULL,NULL,'Брокколи','Брокколи','Раздел: Ингредиенты для салата',false),
	 ('c2201b6d-8bef-535a-8f0e-1487fdef6966',NULL,NULL,NULL,'Брюссельская капуста','Брюссельская капуста','Раздел: Ингредиенты для салата',false),
	 ('52a9f464-487d-5694-950f-ad8055950445',NULL,NULL,NULL,'Ваниль','Ваниль','у меня порошок из сушёных стручков; экстракт/паста/семена; Раздел: Чизкейк',false),
	 ('b238e952-6b98-59ed-b16e-607a509ada2e',NULL,NULL,NULL,'Вареная сгущенка','Вареная сгущенка',NULL,false),
	 ('c28bad38-775f-51c8-9d09-877e3b039384',NULL,NULL,NULL,'Вешенки','Вешенки','или шампиньоны; Раздел: Ингредиенты для начинки с картошкой и грибами',false),
	 ('12b65300-9582-587b-80fa-30d4e720f95e',NULL,NULL,NULL,'Вода','Вода','Раздел: Для теста; Раздел: Ингредиенты для риса из цветной капусты',false),
	 ('e01d8bf5-b6bc-54c3-a3d7-9d6ddc746508',NULL,NULL,NULL,'Воздушный амарант','Воздушный амарант',NULL,false),
	 ('0358f024-c462-55a5-8295-63b999c64af6',NULL,NULL,NULL,'Готовая фасоль','Готовая фасоль',NULL,false),
	 ('15e4f6d6-7076-55da-be9f-716e7fdc9dcd',NULL,NULL,NULL,'Грецкие орехи','Грецкие орехи',NULL,false),
	 ('2235fb17-4534-5d00-8535-0c1ca798e9d9',NULL,NULL,NULL,'Грибы','Грибы','Раздел: Для котлет; шампиньоны/вешенки',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('0a02c657-918d-571d-bcc7-638b4a1dd439',NULL,NULL,NULL,'Для блинов: блины "жюльен"','Для блинов: блины "жюльен"','Раздел: Где ещё использовать',false),
	 ('f44e3551-3273-5986-a3fc-8f8ef023f827',NULL,NULL,NULL,'Заварная часть','Заварная часть','Раздел: Сметанный крем',false),
	 ('c372b164-b120-5058-aec0-b9bd6cb0e974',NULL,NULL,NULL,'Зелень','Зелень','укроп, кинза',false),
	 ('e9ff9e8a-992c-50d5-8adc-e3a8c177c473',NULL,NULL,NULL,'Зелёная стручковая фасоль','Зелёная стручковая фасоль','Раздел: Ингредиенты для салата',false),
	 ('0ac426d9-0a4f-569c-89f3-e36e89d6c036',NULL,NULL,NULL,'Зелёный горошек','Зелёный горошек',NULL,false),
	 ('fc025614-2325-583a-96de-e0414ca683f6',NULL,NULL,NULL,'Зира','Зира','кумин; Раздел: Для котлет',false),
	 ('ff2ccb17-41b5-5d43-b248-96ff694e30a0',NULL,NULL,NULL,'Имбирь','Имбирь','Раздел: Ингредиенты для риса из цветной капусты; Раздел: Ингредиенты для заправки',false),
	 ('3ef92a25-a13a-5092-ab2e-ca888882145c',NULL,NULL,NULL,'Кабачок','Кабачок','кабачок',false),
	 ('48c8e9bf-f165-5a36-b310-222557ee72fb',NULL,NULL,NULL,'Капуста белокочанная','Капуста белокочанная',NULL,false),
	 ('22f9818d-0447-5fb7-b7c9-0c363bc291cd',NULL,NULL,NULL,'Капуста квашеная','Капуста квашеная','Раздел: Ингредиенты для начинки с фаршем из фасоли',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('5474fb48-c744-54be-907e-eab24e35113a',NULL,NULL,NULL,'Карри','Карри','Раздел: Ингредиенты для риса',false),
	 ('6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',NULL,NULL,NULL,'Картофель','Картофель','Раздел: Для котлет; Раздел: Ингредиенты для начинки с картошкой и грибами; Раздел: Вариант 2',false),
	 ('a4cf9371-34fc-5ed9-89eb-89919f1a7ae2',NULL,NULL,NULL,'Картофельное тесто','Картофельное тесто','Раздел: Сборка пиццы',false),
	 ('6748e8ff-7d0f-5397-82ab-476688288098',NULL,NULL,NULL,'Картофельный крахмал','Картофельный крахмал','Раздел: Ингредиенты на 6 кексов',false),
	 ('991a021e-f869-533e-9aca-6ce7cb2a56f6',NULL,NULL,NULL,'Кешью','Кешью','сухой; Раздел: Чизкейк',false),
	 ('7f9c1e58-9d54-5561-a922-c075d743c9d9',NULL,NULL,NULL,'Кинза','Кинза','Раздел: Для котлет',false),
	 ('13900dce-3e69-555f-b088-0de3c5191a04',NULL,NULL,NULL,'Киноа','Киноа',NULL,false),
	 ('4c95ad06-5dc8-5aca-95f1-4e5ec6ca6abe',NULL,NULL,NULL,'Клубника','Клубника','Раздел: Клубничная начинка; Раздел: Ингредиенты на 6 кексов',false),
	 ('9f32f189-d775-5836-a228-8cc178ad6b61',NULL,NULL,NULL,'Клюква вяленая','Клюква вяленая',NULL,false),
	 ('af17f6a2-686a-5f33-bbcb-7eca5f7e8253',NULL,NULL,NULL,'Кокосовая паста','Кокосовая паста','урбеч',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('a8f06049-4e76-5faf-b56f-8390febd646a',NULL,NULL,NULL,'Сметана кокосовая','Сметана кокосовая','Раздел: Сметанный крем',false),
	 ('080247c9-68d3-5101-bb73-c46ae33f9172',NULL,NULL,NULL,'Кокосовая стружка','Кокосовая стружка','чем она жирнее и свежее, тем вкуснее',false),
	 ('bdb1f62b-9795-5f76-93ed-d821c53847af',NULL,NULL,NULL,'Кокосовое масло','Кокосовое масло','Раздел: Для теста',false),
	 ('901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',NULL,NULL,NULL,'Кокосовое молоко','Кокосовое молоко','Раздел: Карамельные яблоки',false),
	 ('28f6f887-a6bc-5516-8224-9d3e1ab0d1fd',NULL,NULL,NULL,'Кокосовые аминокислоты','Кокосовые аминокислоты','Раздел: Ингредиенты для «бекона»; Раздел: Для котлет',false),
	 ('3dcb7477-4d6c-5511-a5bd-0e87d40c7258',NULL,NULL,NULL,'Кокосовые сливки','Кокосовые сливки','Раздел: Чизкейк',false),
	 ('66516ef7-5f43-5dd5-810f-7599a6f5dfaf',NULL,NULL,NULL,'Сахар кокосовый','Сахар кокосовый','Раздел: Карамельные яблоки; Раздел: Клубничная начинка',false),
	 ('bf5422ac-1f24-5072-a884-a95f187f4125',NULL,NULL,NULL,'Кокосовый урбеч','Кокосовый урбеч','паста',false),
	 ('55d9459f-854a-5c23-9b9a-c7e483c2ad3c',NULL,NULL,NULL,'Консервированная белая фасоль','Консервированная белая фасоль','Раздел: 1 вариант',false),
	 ('e339dae4-1a9d-58e4-99a8-ddc9111c5498',NULL,NULL,NULL,'Кориандр молотый','Кориандр молотый',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('f686acf2-e4eb-5a42-8406-eec1f40b66bb',NULL,NULL,NULL,'Корица','Корица','Раздел: Карамельные яблоки',false),
	 ('5810a4fc-11b8-52ab-b0ea-964b8cdd922f',NULL,NULL,NULL,'Крахмал','Крахмал','Раздел: Для теста',false),
	 ('0f4e75e9-6d57-5882-bdb7-13668f79ea15',NULL,NULL,NULL,'Крахмал тапиоки','Крахмал тапиоки','Раздел: Ингредиенты для кекса',false),
	 ('e91a3f6f-3727-5704-84f2-52632fe4e5a6',NULL,NULL,NULL,'Ксантановая камедь','Ксантановая камедь',NULL,false),
	 ('059b624a-58b3-57d4-b265-60297c25b705',NULL,NULL,NULL,'Кумин','Кумин','зира',false),
	 ('460c3eb1-42a4-51f8-9f19-adc6c7c31d50',NULL,NULL,NULL,'Кунжут','Кунжут','Раздел: Ингредиенты для заправки',false),
	 ('20bf6981-7489-5bad-950d-061c1bb5c974',NULL,NULL,NULL,'Куркума','Куркума','Раздел: Ингредиенты для риса из цветной капусты',false),
	 ('936a779e-cd4b-5155-a500-60406cbd0c94',NULL,NULL,NULL,'Кэроб','Кэроб',NULL,false),
	 ('6e0729ea-b639-5cbf-81bb-62077b288e9a',NULL,NULL,NULL,'Лимонный сок','Лимонный сок','Раздел: Ингредиенты для заправки; Раздел: Вариант 2',false),
	 ('202b5aa5-f84b-5069-925d-75e1d4689953',NULL,NULL,NULL,'Листы нори','Листы нори','сушёные без масла, нарезать на небольшие прямоугольники',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('9a346879-8bfd-5ec0-9ece-3fd7d055b41a',NULL,NULL,NULL,'Лук','Лук','Раздел: Для котлет',false),
	 ('c8403fed-a2c7-5fe7-b6bf-7abc0318a2df',NULL,NULL,NULL,'Лук зелёный','Лук зелёный','Раздел: Ингредиенты для заправки',false),
	 ('bf06a3d3-930d-53ef-9b07-25e3258628b1',NULL,NULL,NULL,'Лук красный','Лук красный',NULL,false),
	 ('13b4b933-fc3a-50e0-b7e6-98e7846bdc5a',NULL,NULL,NULL,'Лук сушеный','Лук сушеный','Раздел: Вариант 2',false),
	 ('32a10b45-e313-5e70-8494-75e4f950046c',NULL,NULL,NULL,'Любые на ваш выбор','Любые на ваш выбор','изюм, вишня, клюква, инжир, курага, чернослив',false),
	 ('bf8d085d-86d2-5160-b280-3f07544f5528',NULL,NULL,NULL,'Мак','Мак',NULL,false),
	 ('bdad705e-0503-5cd1-ae9a-1a7c2b042e1b',NULL,NULL,NULL,'Манго','Манго','Раздел: Для глазури',false),
	 ('0de53086-87c0-5c69-8bbf-76e02e762c75',NULL,NULL,NULL,'Маринад от оливок','Маринад от оливок',NULL,false),
	 ('6d7c7c6d-484e-5382-93a4-fb11a6297228',NULL,NULL,NULL,'Марципан','Марципан','Раздел: Ингредиенты для кекса',false),
	 ('57aeb289-29b4-53a8-a2fa-7d829d726779',NULL,NULL,NULL,'Масло виноградной косточки','Масло виноградной косточки',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('14bd91a5-5046-5625-b8d6-9092ba5dda87',NULL,NULL,NULL,'Масло растительное','Масло растительное',NULL,false),
	 ('1c3ae44c-6348-57ed-9ea6-3b0773b3e0ed',NULL,NULL,NULL,'Мешочек для молока','Мешочек для молока','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.',false),
	 ('62c68ca9-8b5c-56aa-8b73-0afe7d8ceac6',NULL,NULL,NULL,'Миндаль','Миндаль',NULL,false),
	 ('01ae364e-e413-58aa-bd32-fe30202262ce',NULL,NULL,NULL,'Миндаль жареный в начинку','Миндаль жареный в начинку',NULL,false),
	 ('742b1e2c-5773-5aaa-b50e-4ab543be0b01',NULL,NULL,NULL,'Миндальная мука','Миндальная мука','Раздел: Для теста',false),
	 ('b571b6f7-732a-5e48-bc2a-0698b9a88bfb',NULL,NULL,NULL,'Миндальное молоко','Миндальное молоко','Раздел: Заварная часть крема',false),
	 ('576705f0-eaa9-566f-b59c-85b72ca84ba7',NULL,NULL,NULL,'Миндальные лепестки','Миндальные лепестки',NULL,false),
	 ('002952e4-01f8-5883-acb7-9a1e3e7c7fa0',NULL,NULL,NULL,'Молоко','Молоко','Раздел: Ингредиенты для кекса',false),
	 ('2c3c04f0-535b-564a-9561-deba13ded4ae',NULL,NULL,NULL,'Молоко ореховое','Молоко ореховое',NULL,false),
	 ('da004df1-ec9f-58cd-a1b6-7c4f272fce7a',NULL,NULL,NULL,'Молоко растительное','Молоко растительное','так как в рецепте нет масла, лучше брать более жирное, у меня кокосовое',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('c0b08006-0854-5fc8-9e94-4c07e59f35d0',NULL,NULL,NULL,'Морковь','Морковь','Раздел: Ингредиенты для кекса',false),
	 ('190930b0-2b41-554f-b454-0e92b1a8e1fb',NULL,NULL,NULL,'Мука бурого риса','Мука бурого риса',NULL,false),
	 ('d6a1eeb1-b75b-5365-ac54-dbac72700a31',NULL,NULL,NULL,'Мука зеленой гречки','Мука зеленой гречки','Раздел: Ингредиенты для кекса',false),
	 ('485c8656-bc51-510e-b851-5c4b7d976a14',NULL,NULL,NULL,'Мука зеленых бананов','Мука зеленых бананов',NULL,false),
	 ('d854a993-11cf-5ffd-bbca-d1c059ac666f',NULL,NULL,NULL,'Мука нутовая','Мука нутовая',NULL,false),
	 ('fe445b5d-0f48-52a7-9949-6751f7b5286b',NULL,NULL,NULL,'Мука овсяная','Мука овсяная',NULL,false),
	 ('83918035-d6e2-5d58-9f5f-e75b7aee1fa9',NULL,NULL,NULL,'Мука пшенная','Мука пшенная',NULL,false),
	 ('1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',NULL,NULL,NULL,'Мука рисовая','Мука рисовая',NULL,false),
	 ('e9beccc4-222a-5078-9b90-f6cd1360bd67',NULL,NULL,NULL,'Мука чечевицы','Мука чечевицы',NULL,false),
	 ('9874594b-71a3-50cc-b314-09e52b719724',NULL,NULL,NULL,'Мускатный орех','Мускатный орех',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('8b92da5b-892a-5c28-ae06-182d254687b2',NULL,NULL,NULL,'Мед','Мед',NULL,false),
	 ('e2c11382-0412-5636-95f9-86c53b83629e',NULL,NULL,NULL,'Начинка','Начинка','сухофрукты, орехи',false),
	 ('facb1701-378a-5464-8d47-0125500e0f3a',NULL,NULL,NULL,'Начинкажюльен грибной','Начинкажюльен грибной',NULL,false),
	 ('85759161-d1a1-5eec-af9b-1d341026533d',NULL,NULL,NULL,'Нут','Нут','Раздел: Ингредиенты для карри',false),
	 ('c9702512-8b24-5ee1-ba35-edd850041a3c',NULL,NULL,NULL,'Овощи по выбору','Овощи по выбору','у меня тыква и цукини; Раздел: Ингредиенты для карри',false),
	 ('7645fe13-3398-5057-a216-e91cf06ef461',NULL,NULL,NULL,'Овощной бульон','Овощной бульон',NULL,false),
	 ('8460acd2-6b82-56df-96d3-29a8605a182b',NULL,NULL,NULL,'Овсяные сливки','Овсяные сливки',NULL,false),
	 ('23773894-cdfb-5005-b5a6-0c76a5bf02f6',NULL,NULL,NULL,'Овсяные хлопья','Овсяные хлопья',NULL,false),
	 ('a436c550-c6e6-5049-9319-3ede60d718cb',NULL,NULL,NULL,'Овсяные хлопья без глютена','Овсяные хлопья без глютена',NULL,false),
	 ('477a150b-c7d7-5845-84e6-6ef853dca327',NULL,NULL,NULL,'Огурцы','Огурцы','Раздел: Ингредиенты для салата',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('352d8afb-0f97-5de2-a460-968e36ecf244',NULL,NULL,NULL,'Оливки','Оливки',NULL,false),
	 ('22e69402-41ae-5cd0-87ec-e68447cc11e5',NULL,NULL,NULL,'Орегано','Орегано','Раздел: Ингредиенты для соуса',false),
	 ('97beb515-943d-5a8b-9100-2f7a2471ba87',NULL,NULL,NULL,'Орехи','Орехи',NULL,false),
	 ('ed426749-cf2d-555d-ac6a-eea3365deae5',NULL,NULL,NULL,'Орехи грецкие','Орехи грецкие','или любые по вкусу',false),
	 ('2fc83e21-d021-5646-acf2-74e7f1745fad',NULL,NULL,NULL,'Острый перец','Острый перец','Раздел: Ингредиенты для «бекона»',false),
	 ('928ef100-3feb-5e17-9351-d5a42f58aaed',NULL,NULL,NULL,'Пажитник молотый','Пажитник молотый','по желанию; Раздел: Ингредиенты для начинки с фаршем из фасоли',false),
	 ('c2144e2f-7e15-5b03-bb98-5a2c88e1ad9a',NULL,NULL,NULL,'Паприка','Паприка','Раздел: Вариант 2',false),
	 ('389cb4b0-c501-5f31-a2ca-9c0c5d1d2f8d',NULL,NULL,NULL,'Паприка копченая','Паприка копченая','натурального копчения; Раздел: Барбекю соус',false),
	 ('53c35600-d58b-51bb-b65b-8998aab99d22',NULL,NULL,NULL,'Паста ореховая','Паста ореховая','фундук, миндаль, кешью, подсолнечник, кокосовая паста и т.д.',false),
	 ('6ecb4c22-d683-525f-9bd5-1a4564818887',NULL,NULL,NULL,'Томатная паста','Томатная паста','Раздел: Ингредиенты для начинки с фаршем из фасоли; Раздел: Барбекю соус',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('b07f8f1d-df35-58c0-b3a0-dc827192505f',NULL,NULL,NULL,'Пекинская капуста','Пекинская капуста',NULL,false),
	 ('f2e9c448-c9cf-5f14-9a95-91235d826ab3',NULL,NULL,NULL,'Пергамент для выпечки','Пергамент для выпечки','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.',false),
	 ('02922fee-9ec2-56b8-8491-3d36eaca7335',NULL,NULL,NULL,'Перец белый','Перец белый',NULL,false),
	 ('e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',NULL,NULL,NULL,'Перец сладкий','Перец сладкий',NULL,false),
	 ('3b57745e-ccdb-5972-ac50-7f5247e08655',NULL,NULL,NULL,'Перец сладкий запеченный','Перец сладкий запеченный',NULL,false),
	 ('67816b8d-b42a-5d1d-b5be-144282df4927',NULL,NULL,NULL,'Перец чили свежий','Перец чили свежий','по желанию',false),
	 ('4e5188eb-ac95-502a-81cf-8a4a7973d1d6',NULL,NULL,NULL,'Перец черный','Перец черный','Раздел: Ингредиенты для начинки с картошкой и грибами',false),
	 ('93e3f557-a326-5b31-9bcf-2b6a1c9ca04a',NULL,NULL,NULL,'Перец черный молотый','Перец черный молотый','Раздел: Ингредиенты для начинки с фаршем из фасоли',false),
	 ('ecf08b78-17c0-503b-9ef7-26108fb75e91',NULL,NULL,NULL,'Петрушка','Петрушка',NULL,false),
	 ('7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',NULL,NULL,NULL,'Помидор','Помидор','очищенные/протёртые',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('6b4ba6e3-74e2-54a9-8879-edf13451b244',NULL,NULL,NULL,'Помидоры черри','Помидоры черри',NULL,false),
	 ('a72af268-3dd8-58d1-be0b-42bcfd48ef6b',NULL,NULL,NULL,'Протертые томаты','Протертые томаты',NULL,false),
	 ('5c3e9db6-3f89-535f-9035-8e2b04ec6ce0',NULL,NULL,NULL,'Пряности','Пряности','Раздел: Ингредиенты для кекса',false),
	 ('b887a3d4-5bd4-5746-9ecc-92553a79663b',NULL,NULL,NULL,'Псиллиум','Псиллиум','цельный',false),
	 ('36aee9fe-196d-5b77-9b60-1a100466e652',NULL,NULL,NULL,'Псиллиум цельный','Псиллиум цельный',NULL,false),
	 ('1999e680-7ac5-5285-b615-94b4ad501e77',NULL,NULL,NULL,'Псиллиум шелуха','Псиллиум шелуха',NULL,false),
	 ('3a1cd39e-cec7-543b-9759-bde7daec371d',NULL,NULL,NULL,'Пшено','Пшено','Раздел: Для котлет',false),
	 ('c608611c-22bd-5e37-a603-afa0f6525a5f',NULL,NULL,NULL,'Яблочное пюре','Яблочное пюре','Раздел: Для теста; Раздел: Барбекю соус; готовое или самодельное из запечённых зелёных яблок',false),
	 ('cb3b977b-e4c9-53fa-9383-1b716de9d08c',NULL,NULL,NULL,'Разрыхлитель','Разрыхлитель','Раздел: Ингредиенты для кекса',false),
	 ('6a3a0636-a3c4-5eed-95a5-2593d2dffeca',NULL,NULL,NULL,'Рис','Рис','Раздел: Ингредиенты для риса',false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('40fcbd45-bae4-54f3-8d9c-595d86745f43',NULL,NULL,NULL,'Рисовая бумага','Рисовая бумага',NULL,false),
	 ('db6ef57e-caee-5092-8010-e04ef42a904c',NULL,NULL,NULL,'Рукола','Рукола',NULL,false),
	 ('1022bab1-90d2-57be-add0-cd9aa9d0a423',NULL,NULL,NULL,'Салат фриллис','Салат фриллис','Раздел: Ингредиенты для салата',false),
	 ('a58bff9f-b034-53c9-9a5b-2932e7f61d26',NULL,NULL,NULL,'Сахар','Сахар','Раздел: Ингредиенты для кекса',false),
	 ('a9b0e13a-3138-5a17-a37b-e09bed645d1e',NULL,NULL,NULL,'Сахар панела','Сахар панела','Раздел: Для теста',false),
	 ('9fbb86ee-092d-5fa9-b4dc-e845e3019b89',NULL,NULL,NULL,'Сахар тростниковый','Сахар тростниковый',NULL,false),
	 ('d56b7753-5bb3-5465-9b6b-d5a9a964688b',NULL,NULL,NULL,'Свекла','Свекла',NULL,false),
	 ('a5593de5-0438-5a7d-9c70-cd49195facf4',NULL,NULL,NULL,'Семена амаранта','Семена амаранта','или киноа',false),
	 ('6a4daef6-0e4c-5b49-ac40-c882869fbf13',NULL,NULL,NULL,'Семена горчицы','Семена горчицы','Раздел: Барбекю соус',false),
	 ('ee3cce66-8244-5982-ad5d-a8b5eaedac23',NULL,NULL,NULL,'Семена льна','Семена льна',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('87ebb42f-67b1-5e5e-9ae5-151955c6d64b',NULL,NULL,NULL,'Сироп','Сироп',NULL,false),
	 ('65c5a320-8738-50ac-b17a-5c203b6ac1c1',NULL,NULL,NULL,'Сироп топинамбура','Сироп топинамбура','Раздел: Чизкейк',false),
	 ('487a8345-e9e6-582c-8bd3-f4b0ad244cd6',NULL,NULL,NULL,'Сметанный крем','Сметанный крем',NULL,false),
	 ('312cb550-e8a8-5271-89c3-06d8ed9359cb',NULL,NULL,NULL,'Сода','Сода','Раздел: Ингредиенты для кекса',false),
	 ('fa35a350-38aa-51ec-9185-46bcc2c07bb8',NULL,NULL,NULL,'Сок гранатовый','Сок гранатовый',NULL,false),
	 ('f43099f3-ac3a-53ba-8084-8bca3e764dd0',NULL,NULL,NULL,'Сок и цедра','Сок и цедра',NULL,false),
	 ('a7b21dea-293a-5652-96b4-72716b5ee432',NULL,NULL,NULL,'Сок из-под сухофруктов','Сок из-под сухофруктов','Раздел: Ингредиенты для кекса',false),
	 ('914e0df9-971c-51e3-a314-5031c16cea03',NULL,NULL,NULL,'Соль','Соль','для оттенения сладости; Раздел: Барбекю соус',false),
	 ('f8715b4e-305d-5318-953f-ac8cc71d77c1',NULL,NULL,NULL,'Соль по вкусу','Соль по вкусу','Раздел: Ингредиенты для карри',false),
	 ('1c73179c-f246-56f5-84b3-a7f222449b7d',NULL,NULL,NULL,'Соль, перец','Соль, перец',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('430094cf-2d4a-5e17-b18c-5b2edde4c715',NULL,NULL,NULL,'Соус барбекю','Соус барбекю','Раздел: Для котлет',false),
	 ('2c8db663-98b8-59ae-83e9-376408cb5347',NULL,NULL,NULL,'Спаржа','Спаржа','Раздел: Ингредиенты для салата',false),
	 ('69e5987e-6cbd-5583-b42d-d2f86710fe22',NULL,NULL,NULL,'Специи','Специи','острый перец или чёрный перец',false),
	 ('c4c520bd-d2db-53a9-874b-989ce03ec69f',NULL,NULL,NULL,'Сухофрукты','Сухофрукты','Раздел: Ингредиенты для кекса',false),
	 ('1ae1e4d0-4d21-5a16-b62f-2c0aab59e785',NULL,NULL,NULL,'Сушеные травы','Сушеные травы','орегано или смесь итальянских трав',false),
	 ('8eb1095b-6eac-5c60-baa7-53e95bf88beb',NULL,NULL,NULL,'Сырный соус из картошки','Сырный соус из картошки',NULL,false),
	 ('a6c35db6-8b66-50c2-8e79-f6aec67143fa',NULL,NULL,NULL,'Тапиоковый крахмал','Тапиоковый крахмал','Раздел: Чизкейк',false),
	 ('55e34641-ed9b-5ae9-9872-7eb2d758c9f2',NULL,NULL,NULL,'Творожный сыр','Творожный сыр',NULL,false),
	 ('c9775cc5-7f53-5501-bea9-9909fd23259a',NULL,NULL,NULL,'Твердый сыр','Твердый сыр',NULL,false),
	 ('cebf6875-7da4-5c8f-8e55-f0b8b990cf21',NULL,NULL,NULL,'Тимьян','Тимьян',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('cddc2c2b-88b8-5c54-8e35-d4168a0af694',NULL,NULL,NULL,'Тмин','Тмин',NULL,false),
	 ('5273c607-82de-578c-a21e-83153982f22e',NULL,NULL,NULL,'Травы','Травы','орегано, итальянская смесь',false),
	 ('56195042-f16f-5822-a651-62d43d3fee37',NULL,NULL,NULL,'Травы итальянские','Травы итальянские','готовая смесь или отдельно базилик, розмарин, орегано, тимьян',false),
	 ('d59627c3-a64c-5c10-acc4-3e702de6706a',NULL,NULL,NULL,'Тыква','Тыква',NULL,false),
	 ('a111617d-d2b4-51d9-adf5-8d74d7a85a41',NULL,NULL,NULL,'Тыквенное пюре','Тыквенное пюре',NULL,false),
	 ('95258fee-371a-561b-9762-92f831d44a36',NULL,NULL,NULL,'Фасоль красная','Фасоль красная','Раздел: Для котлет',false),
	 ('a128b1f3-0b92-50a8-8b66-3b69bc7cc6ca',NULL,NULL,NULL,'Фасоль красная готовая','Фасоль красная готовая','Раздел: Ингредиенты для начинки с фаршем из фасоли',false),
	 ('e833d9f3-06a2-5989-a583-e325837945b5',NULL,NULL,NULL,'Форма для сборки торта','Форма для сборки торта','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.',false),
	 ('f4cd40bd-ce2a-546d-890e-251275ad1828',NULL,NULL,NULL,'Фруктовое','Фруктовое','яблочное, банановое, тыквенное',false),
	 ('9722b993-1d2a-5a85-b88a-7662e86bc7b9',NULL,NULL,NULL,'Хлопья','Хлопья',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('74dba30b-7495-5133-b3b6-efc8865da891',NULL,NULL,NULL,'Хмели-сунели','Хмели-сунели','Раздел: Ингредиенты для начинки с фаршем из фасоли',false),
	 ('3ee68412-100f-5fae-a89a-a416fa5f1beb',NULL,NULL,NULL,'Цветная капуста','Цветная капуста','Раздел: Вариант 2',false),
	 ('e0e8a13c-abfd-55b0-9199-41c301eb5f6d',NULL,NULL,NULL,'Цедра апельсина','Цедра апельсина',NULL,false),
	 ('ac2b5831-f80b-56ce-bb00-014f1a40bf76',NULL,NULL,NULL,'Цедра апельсина и лимона','Цедра апельсина и лимона','по желанию, для ещё большей ароматности; Раздел: Ингредиенты для кекса',false),
	 ('a30ae4ef-07f9-502f-ad14-c1ed5ca5042b',NULL,NULL,NULL,'Цедра лимона','Цедра лимона',NULL,false),
	 ('64a97ac3-4cdf-5198-ba0e-e33ff161050a',NULL,NULL,NULL,'Цукаты апельсиновые','Цукаты апельсиновые',NULL,false),
	 ('a7536601-aa13-59ab-ac5b-0f2f04e9050f',NULL,NULL,NULL,'Чеснок','Чеснок','Раздел: Для котлет',false),
	 ('4ed1a2d5-6ff6-5273-a230-0905784ab869',NULL,NULL,NULL,'Чеснок сушеный','Чеснок сушеный','Раздел: Барбекю соус',false),
	 ('23c3e842-e0e2-5e15-887f-c38f188a2fe7',NULL,NULL,NULL,'Чечевица красная','Чечевица красная',NULL,false),
	 ('a62b2882-7b48-5fb7-9fdc-86c21a327fba',NULL,NULL,NULL,'Черная смородина','Черная смородина',NULL,false);
INSERT INTO public."Ingredients" ("Id","UsageComment","PhotoId","RecipeId","Name","Description","Comment","IsDeleted") VALUES
	 ('d7946e27-c902-5271-8e8b-78a80714b651',NULL,NULL,NULL,'Черный перец','Черный перец',NULL,false),
	 ('d4495cb8-729c-51ed-bfbd-985a867bb2a9',NULL,NULL,NULL,'Шампиньоны','Шампиньоны','Раздел: Ингредиенты для «бекона»',false),
	 ('0a463f3f-f8fa-5dd3-9f28-d39b08d2373e',NULL,NULL,NULL,'Шпинат','Шпинат',NULL,false),
	 ('c2af850e-9593-5627-acc6-92eed80dd7eb',NULL,NULL,NULL,'Яблоко','Яблоко','Раздел: Карамельные яблоки',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63',NULL,NULL,NULL,'00:45:00',NULL,'Паста ореховая,Сухофрукты,Сушённые ягоды,Корица,Кардомон,Имбирь,Гвоздика,Орехи,Семена,Кокосовая стружка,Воздушный амарант,Киноа,Фруктовое пюре,Овощное пюре; можно добавлять и комбинировать по вкусу','Хранить в закрытой банке/контейнере до 1 месяца; Хранить в закрытой банке/контейнере до 1 месяца','есть с растительным молоком / йогуртом / творожком / банановым молоком / мороженым; топпинг к смузи-боулам; добавка к фруктовым салатам; запекать с ней фрукты (яблоки, груши, персики, абрикосы, сливы) и ягоды; добавлять в батончики и конфеты','Гранола','крупы,подсластитель,жиры опциональны',NULL,false),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e',NULL,NULL,NULL,'00:20:00',NULL,NULL,NULL,'Посыпать смузи-боулы и каши; Добавлять в домашние конфеты и батончики; Смешивать с гранолой; Украшать десерты; Использовать как хрустящий элемент в салатах','Воздушный амарант','крупы','"Попкорн" из амаранта, который можно использовать как составляющую десертов и несладких блюд, как декор',false),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',NULL,NULL,NULL,'01:35:00',NULL,'Вы можете экспериментировать с наполнением: заменять картошку на батат/морковь; полностью её убрать; делать котлеты более овощными, добавляя отварные овощи (брокколи, цветную капусту, обычную капусту, зелёную фасоль, горошек); в конце видео показан вариант сборки бургера','Такие котлеты можно заготовить в большом количестве и заморозить в сыром или запечённом виде. Затем разморозить в духовке или запечь до готовности; хранить до 3 месяцев',NULL,'Котлеты для бургера','бобы,крупы',NULL,false),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',NULL,NULL,NULL,'01:30:00',NULL,'Цукини',NULL,NULL,'Лазанья из кабачков','Лазанья из кабачков',NULL,false),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',NULL,NULL,NULL,'00:55:00',NULL,NULL,NULL,NULL,'Пшённые тортильи','крупы',NULL,false),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',NULL,NULL,NULL,'01:55:00',NULL,'Вместо готовой муки можно также использовать цельные бобы и крупы . Для этого чечевицу, гречку и пшено нужно промыть, залить холодной водой и оставить на ночь или несколько часов . Затем снова промыть и сложить в блендер с остальными ингредиентами (пюре, сода, соль, крахмал, семена льна) и взбить до максимально однородной консистенции. Здесь дополнительная вода не нужна','Хранить в бумажном пакете, завёрнутом в полиэтилен. Так хлеб будет меньше высыхать; Хранить в бумажном пакете, завёрнутом в полиэтилен',NULL,'Тыквенный хлеб','бобы,крупы,крахмал','Этот рецепт специально разработан без псиллиума. Несмотря на это, мягкая текстура без сухости достигается благодаря тыкве и льняным семенам.',false),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528',NULL,NULL,NULL,'00:30:00',NULL,'Тахини; По желанию можно добавить тахини (кунжутную пасту) или авокадо',NULL,NULL,'Хумус из зелёного горошка','Хумус из зелёного горошка',NULL,false),
	 ('98638408-7fec-598e-90a3-eba036fa54df',NULL,NULL,NULL,'00:55:00',NULL,'Кокосовое молоко,Кешью,Томаты,Тыква,Цветная капуста; - Для сливочного вкуса при взбивании можно добавить кокосовое молоко, замоченные орехи кешью или кешью-урбеч (по вкусу) - Дополнительно можно добавить другие овощи: запечённый перец, томаты, тыкву, цветную капусту и т.д.',NULL,'дип к овощам и зелени; соус для лепёшек, хлебцев, сэндвичей, бургеров; соус к пасте, лазанье; мак-н-чиз, запечённые овощи (смешать с ним пасту или овощи и запечь); для румяной корочки сверху запеканок; база для крем-супа','Сырный соус из картошки','Сырный соус из картошки','Этот соус универсальный, подходит для многих блюд. Благодаря картошке получается тянущимся',false),
	 ('a6a19570-7d3d-5f04-b1a4-7964199d2b55',NULL,NULL,NULL,'00:50:00',NULL,NULL,NULL,NULL,'Баклажановая намазка','жиры',NULL,false),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b',NULL,NULL,NULL,'01:15:00',NULL,'Крахмал тапиоки можно заменить на картофельный или не добавлять',NULL,NULL,'Шарлотка на сковороде','бобы,орехи,подсластитель,жиры','Этот рецепт подходит, конечно, и для духовки (будет немного быстрее и удобнее), но если у вас её нет, то по нему вы сможете приготовить пирог и на плите! Также этот рецепт для случаев, если вы не используете рисовую муку или ограничиваете крупы в целом',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',NULL,NULL,NULL,'05:10:00',NULL,NULL,NULL,NULL,'Яблочный чизкейк','крупы,кокос,орехи,жиры,подсластитель,крахмал','Рецепт рассчитан на форму примерно 20 см диаметром. Можно использовать чуть меньше или чуть больше, от этого будет зависеть высота бортиков. Подойдёт как кондитерское кольцо, так и любая форма с дном, разъёмная или обычная; в конце видео показан вариант приготовления яблочных корзиночек',false),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',NULL,NULL,NULL,'04:40:00',NULL,'Манго можно не добавлять и сделать более классический вариант батончиков. Тогда добавьте чуть больше сиропа (по вкусу) и 1/2 стакана кокосовых сливок, остальные пропорции оставьте как есть; Если вы употребляете шоколад, можно глазировать батончики веганским шоколадом, самодельным или покупным. Во втором случае нужно добавить в шоколад немного какао масла, чтобы глазурь была более текучая ---','Хранить в холодильнике в контейнере до 3-5 дней',NULL,'Баунти','кокос,подсластитель,жиры',NULL,false),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',NULL,NULL,NULL,'02:30:00',NULL,'Сироп кленовый,Мёд','Хранить в холодильнике до 3-5 дней или в морозилке в герметичной ёмкости/пакете до 1 месяца',NULL,'Рафаэлло','кокос,жиры,подсластитель','Действительно райские, нежные конфеты с идеальным сочетанием вкусов из 5 ингредиентов; По этому принципу вы можете делать конфеты и на основе других фруктово-ягодных пюре, регулируя сладость и количество агара для загущения; Для хрустящего корпуса: сначала охладить готовые конфеты несколько минут в морозилке, затем окунуть в растопленное какао масло и сразу обвалять в стружке',false),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',NULL,NULL,NULL,'01:20:00',NULL,'Рис,Киноа,Фунчоза,Соус терияки; Вы можете экспериментировать с составом начинки, добавляя другие овощи, а также, например, рис, киноа, фунчозу; Отличным дополнением к спринг-роллам будет: Соус "терияки"',NULL,NULL,'Запечённые спринг-роллы','крупы,жиры опциональны',NULL,false),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7',NULL,NULL,NULL,'00:40:00',NULL,'Вместо соуса я использовала воду от замоченных сушёных томатов (без масла): горсть томатов залить горячей водой на 15-20 минут, добавить сок 1/4 апельсина + сок 1/4 лимона','хранить в холодильнике',NULL,'Соус "терияки"','крахмал',NULL,false),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',NULL,NULL,NULL,'00:03:00',NULL,'Если делаете полностью ягодную запеканку, возьмите примерно 200 г разморожённых или свежих ягод чёрной смородины. Если разделить массу пополам (как на видео), то в половину добавить 100 г ягод. Также можно пробовать добавлять и другие ягоды. Если они водянистые, после разморозки слейте воду.',NULL,NULL,'«Творожная» запеканка','орехи,жиры,бобы,подсластитель','Довольно плотная запеканка на основе кешью и нута, напоминает творожную, без выраженных привкусов. Можно делать классическую или добавлять ягоды - я совместила два слоя. Смородина здесь отлично подходит, она добавила кислинку и лёгкий ягодный вкус',false),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',NULL,NULL,NULL,'02:00:00',NULL,NULL,NULL,NULL,'Фаршированные баклажаны','Фаршированные баклажаны','Простой, но насыщенный и сочный рецепт!',false),
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c',NULL,NULL,NULL,'00:40:00',NULL,NULL,NULL,NULL,'Марципан','орехи,подсластитель,жиры',NULL,false),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',NULL,NULL,NULL,'02:50:00',NULL,'Гранатовый сок; - Сок можно использовать в жидкую часть теста - Если сухофрукты очень влажные, лучше просушить их на бумажной салфетке ---',NULL,NULL,'Морковный кекс с марципаном','крупы,орехи,подсластитель,жиры','Этот кекс я сделала максимально рождественским, добавив в начинку марципан, пропитанные сухофрукты и пряности. Вы можете менять состав под себя, например, можно не добавлять морковь и марципан, брать разные сухофрукты и специи',false),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',NULL,NULL,NULL,'01:00:00',NULL,'- Можно добавлять разные овощи (например, сладкий перец, зелёную фасоль, горошек, батат, брокколи, баклажан), а ананас исключить - Вместо цветной капусты можно взять рис или рисовую лапшу, а вместо нута - другие бобовые или вообще их исключить; Для более густой текстуры можно добавить в молоко 1 ч.л. крахмала ---',NULL,NULL,'Карри с ананасом и «рисом» из цветной капусты','кокос,жиры','Пряный согревающий и сытный рецепт. Вместо обычного риса предлагаю приготовить для гарнира цветную капусту. Можно использовать как базовый рецепт, добавляя разные овощи и гарнир, а ананас исключить',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1',NULL,NULL,NULL,'01:25:00',NULL,'Цукаты,Клюква вяленая,Сухофрукты',NULL,NULL,'Флорентини','кокос,орехи,жиры,подсластитель','Флорентини - это традиционная итальянская выпечка изначально без муки и яиц, представляет собой миндальные лепестки в карамели. Также может содержать другие добавки по желанию Скажу сразу, что это, конечно, довольно жирный и сладкий рецепт, но очень эффектный и простой в приготовлении. Это печенье-козинак прекрасно подойдёт для украшения новогоднего или рождественского стола',false),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',NULL,NULL,NULL,'01:15:00',NULL,'- Для начинки можно использовать и другие овощи - С заливкой тоже можно экспериментировать: делать её более нейтрально-сливочной, убрав морковь и перец; или же добавить, например, томаты. Также такая заливка с овощами подойдёт в качестве начинки для киша',NULL,NULL,'Запеканка со шпинатом и зелёными овощами','бобы,жиры опциональны',NULL,false),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9',NULL,NULL,NULL,'00:50:00',NULL,'Сухофрукты; Рекомендую добавлять апельсиновый сок и цедру для ароматного зимнего варианта, но вы можете делать другие вариации печенья: заменять сок на воду, молоко или яблочное/банановое/тыквенное пюре, регулируя консистенцию',NULL,NULL,'Сконы','крупы,жиры,подсластитель,кокос','Очень простые и невероятно вкусные и ароматные печенья! Рецепт без граммовок для максимально быстрого домашнего приготовления.',false),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',NULL,NULL,NULL,'00:05:00',NULL,NULL,NULL,NULL,'Шоколадная колбаса','крупы,орехи,кокос,жиры,подсластитель','Рисовая мука - 50г Овсяная мука - 50г Миндальная мука - 30г Соль - щепотка Сахар кокосовый - 50г Кокосовое масло - 30г Вода - 25г; Кокосовые сливки - 100г Кэроб - 3-4 ст.л. Кокосовый урбеч - 50г',false),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',NULL,NULL,NULL,'01:00:00',NULL,'- Это универсальный рецепт, в котором можно использовать разные овощи и специи - Ещё один вариант: смешать хлопья с тёртым кабачком и морковью / тушёной капустой; Подавать вкуснее всего с соусом, например, авокадо или кешью ---',NULL,NULL,'Овсяно-овощная запеканка','крупы','На случай, если вам надоела сладкая овсяная каша - предлагаю солёный пряный вариант запечённой овсянки с овощами',false),
	 ('37fef56b-1416-5636-b418-332ded70282e',NULL,NULL,NULL,'00:55:00',NULL,'--- ⏰ Общее время: 45-55 минут 👩🏻‍🍳 Активное время: 25-30 минут ⏳ Ожидание: 20-25 минут ---; Вместо яблочного пюре можно использовать тёртое яблоко, фруктово-ягодные джемы или просто ягоды, смешанные с крахмалом и сахаром; измельчённые сухофрукты с орехами',NULL,NULL,'Рогалики с яблоком и орехами','крупы,жиры,подсластитель,кокос','Песочные рогалики с простым вариантом начинки',false),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',NULL,NULL,NULL,'01:20:00',NULL,'- В начинку можно добавлять разные овощи (сладкий перец, баклажан, цукини, брокколи, зелёный горошек и др.), - Также можно добавить сливочную составляющую, например, кешью-соус или кокосовую моцареллу (она будет слегка тянуться)',NULL,NULL,'Картофельные лепёшки с начинкой','Картофельные лепёшки с начинкой','Просто картошка с грибами, но в более интересном виде, который нравится детям. Это лепёшки с румяной запечённой корочкой и насыщенным вкусом овощей. Удобно брать с собой.',false),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97',NULL,NULL,NULL,'01:20:00',NULL,'Перец острый',NULL,NULL,'Паста карбонара','крупы','Представляю вариант безжировой, но при этом насыщенной пасты! При желании вы можете добавить в соус кокосовое/ореховое молоко или орехи кешью для большей сливочности; Для пасты я использовала 2й вариант соуса с цветной капустой',false),
	 ('1ab9222a-9d4d-5f6c-97a0-4152a2499ee3',NULL,NULL,NULL,'00:10:00',NULL,'Самодельная аквафаба; Из указанного количества получается примерно 700 г аквафабы. Вы можете брать любое количество нута, в зависимости от того, какое количество аквафабы хотите получить. Удобнее делать сразу много и замораживать. #### Приготовление 1. Нут замочить на ночь в холодной воде. 2. Воду слить, промыть, переложить в кастрюлю. 3. Залить водой в указанной пропорции или на глаз, чтобы вода покрывала нут примерно на 3 см выше. 4. Довести до кипения, убавить огонь и варить под крышкой около 1,5 часов. ','Хранить в холодильнике до 3 дней',NULL,'Аквафаба','бобы','Аквафаба - отвар бобовых, который имеет свойство взбиваться, как яичный белок. Для десертов лучше всего работает аквафаба из-под нута или белой фасоли. Есть 3 варианта её получения',false),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',NULL,NULL,NULL,'00:02:00',NULL,'Зелень,Ореховое молоко,Специи; - Это базовый рецепт, в котором вы можете регулировать вкус за счёт добавок или замены кокоса на орехи. А также степень твёрдости регулируется количеством агара и временем уваривания. - Если хотите более мягкий сыр, можно уменьшить количество агара. При этом сыр по текстуре будет похож на плавленный. - Молоко выбирайте более нейтральное по вкусу (без ярко выраженного вкуса кокоса). - По желанию можно добавить в сыр сушёный чеснок, лук, пажитник, сушёные травы, томаты или други','Хранить в закрытом контейнере до 3-5 дней',NULL,'Твёрдый сыр','жиры,кокос,крахмал','Твёрдый сыр, который можно использовать в холодном виде с хлебом, крекерами, для закусок и салатов,',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',NULL,NULL,NULL,'02:00:00',NULL,'Вместо кокосового молока в тесто можно добавить другое растительное молоко, но тогда нужно добавить 20-30г кокосового или другого растительного масла для жирности. Смазать можно любым молоком',NULL,NULL,'Маковый рулет','жиры,крупы,орехи,псиллиум,подсластитель','Ароматный рулет, с большим количеством начинки и тонкими слоями теста',false),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab',NULL,NULL,NULL,'00:50:00',NULL,'~ на 2 небольшие пиццы; Вместо нутовой муки и тапиокового крахмала можно использовать другую безглютеновую муку или смесь разных (рисовая, зелёной гречки, овсяная, кассавы, картофельный крахмал и пр.)',NULL,'пицца на картофельной основе; Кальцоне (закрытая пицца); картофельные пирожки; лепёшка вместо хлеба','Картофельное тесто','Картофельное тесто','Тесто на основе картофельного пюре, подойдёт в качестве основы для пиццы, для пирожков или просто для лепёшки в духовке',false),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5',NULL,NULL,NULL,'00:20:00',NULL,NULL,NULL,'тонкая лепёшка, в которую можно заворачивать любую начинку; вместо хлеба; основа для:Кесадилья, буррито, шаурмы; мягкая толстая лепёшка с овощами (нутовый омлет)','Нутовая лепёшка','бобы,жиры опциональны','Классическая лепёшка на сковороде, которую можно использовать как базу для разных рецептов, варьируя состав и толщину лепёшки; Тонкая хрустящая лепёшка для кесадильи или любых начинок IMG_8349.MOV; ~ на 3 лепёшки (диаметр сковороды 26 см); Крахмал делает лепёшку более тонкой и хрустящей, но вы можете исключить его',false),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',NULL,NULL,NULL,'01:10:00',NULL,'Овощи,Кокосовый сахар,Итальянские травы; - Для начинки можно использовать любые овощи по вкусу - Вместо твёрдого сыра можно использовать любой соус по вашему желанию (например, кешью-соус)',NULL,NULL,'Кальцоне (закрытая пицца)','жиры,крахмал','Закрытая пицца с сочной тягучей начинкой в самом здоровом варианте - с картофельным тестом, овощами и домашним сыром.',false),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',NULL,NULL,NULL,'01:20:00',NULL,'Сухофрукты,Цукаты,Растительное молоко; --- ⏰ Общее время: 1 час 20 минут 👩🏻‍🍳 Активное время: 20-25 минут ⏳ Ожидание: 60 минут ---; В тесто можно добавлять любую начинку по вашему вкусу: изюм, клюква, орехи, апельсиновая или лимонная цедра, цукаты.',NULL,NULL,'Бискотти','крупы,орехи,подсластитель,жиры,крахмал','Традиционное итальянское хрустящее печенье, или наши сухарики.',false),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',NULL,NULL,NULL,'01:35:00',NULL,'Сироп кленовый,Мёд,Апельсиновый сок; - Рис отварить по инструкции. Важно не разварить - рис должен остаться целым и не клейким. Хорошо подойдёт длиннозёрный рис, он получается более рассыпчатым. Можно также брать бурый. - Вместо аминокислот можно взять лимонный сок + соль, корректируя количество сиропа. - Можно добавлять чеснок или любые специи по вкусу.; Это лишь один вариант состава салата, экспериментируйте с наполнением. Подойдут любые зелёные (и не только) овощи, любая зелень. Например, зелёный горошек','хранить заправленный салат, не добавляйте в него рис, иначе он размокнет и потеряет хруст',NULL,'Зелёный салат с хрустящим рисом','крупы',NULL,false),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',NULL,NULL,NULL,'05:00:00',NULL,'--- ⏰ Общее время: 2,5 часа (с запеканием батата) 👩🏻‍🍳 Активное время: 20-25 минут ⏳ Ожидание: 2+ часа (запекание батата, выпекание кекса, остывание) ---; - Батат запечь целиком в кожуре до мягкости. Я рекомендую именно запечённый вариант, так как в нём остаётся меньше влаги и больше вкуса. Но можно также отварить батат на пару. От этого может меняться густота теста и необходимое количество муки. - Также в этом рецепте можно пробовать заменять батат на тыкву, бананы или пюре запечённых яблок - Кокосовый урб',NULL,NULL,'Брауни из батата','орехи,подсластитель,кокос,жиры','В этом рецепте сочетание ингредиентов даёт действительно шоколадный вкус!',false),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',NULL,NULL,NULL,'01:45:00',NULL,'Растительное молоко,Итальянские травы; - Рис нужен белый, более клейкий, например, круглозёрный. Бурый рис не подходит, так как не будет склеиваться - Чечевицу можно не использовать или вместо неё взять отварную фасоль, немного её размяв перед добавлением в начинку - С начинкой можно экспериментировать, добавляя разные овощи и специи. Например, сладкий перец, томаты или вместо обычной капусты - цветную, предварительно измельчив в комбайне; Голубцы можно запечь без заливки в виде котлеток в течение 25-30 мин',NULL,NULL,'Ленивые голубцы','крупы,бобы,жиры опциональны','Эти голубцы с томатно-сливочной заливкой прекрасны как в классическом виде, так и ленивом. Поэтому вы также можете приготовить начинку без капусты, а капустные листы отварить и завернуть в них начинку (при необходимости по инструкции в интернете)',false),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',NULL,NULL,NULL,'01:15:00',NULL,'- Молоко придаёт больше сливочности, аромата, румяности и делает текстуру менее грубой. Можно заменить его на воду, но при этом добавить 1 ст.л. любого растительного масла - Количество сиропа регулируйте по своему вкусу, если вы хотите сладкие сушки. Для несладкого варианта сироп можно исключить. Вместо сиропа можно взять кокосовый или тростниковый сахар, но возможно придётся увеличить количество жидкости (ориентируйтесь на консистенцию теста); Для итальянской версии с травами хорошо подойдёт оливковое масл',NULL,NULL,'Сушки','крупы,жиры','Хрустящие, ароматные, румяные сушки в двух вариантах - сладком и солёном',false),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5',NULL,NULL,NULL,'00:35:00',NULL,'- Грибы можно использовать любые доступные вам. Чем они более ароматные, тем лучше. Можно брать смесь разных грибов. Я брала королевские шампиньоны, они имеют более яркий вкус, чем обычные. - Чтобы суп получился более насыщенным, лучше использовать овощной бульон. В бульон можно добавить любые овощи, в том числе грибы для более яркого грибного вкуса (можно добавить сушёные). Лук, чеснок, морковь, сельдерей, грибы, зелень - база насыщенного бульона. - Если вы по какой-то причине хотите заменить овсяные сливк',NULL,NULL,'Грибной сливочный суп','Грибной сливочный суп','Этот густой насыщенный суп получается сливочным без кокоса и орехов. Домашние овсяные сливки делают его шелковистым и при этом нежирным. Благодаря свойствам овсянки не нужно загущать суп крахмалом или мукой.',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74',NULL,NULL,NULL,'01:20:00',NULL,'- Количество специй, соли и сока корректируйте по вкусу - Салат состоит из 2 основых частей: свёкла как база + маринованный лук с зеленью и специями. В качестве базы можно использовать разные овощи и добавлять к ним такой лук, а также экспериментировать со специями и зеленью',NULL,NULL,'Свекольный грузинский салат','орехи,жиры','Насыщенный и сочный салат из свёклы по-грузински',false),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',NULL,NULL,NULL,'01:05:00',NULL,'Крахмал тапиоки; - Нутовую муку можно заменить на муку кассавы или овсяную, но текстура будет отличаться - Можно не добавлять клубнику или заменить на другие ягоды (вишня, черника, смородина - их можно брать замороженными или свежими)',NULL,NULL,'Клубнично-кокосовые кексы','кокос,крупы,жиры,бобы','Кексы с сочным мякишем. В составе нет подсластителей, только банан, поэтому сладость очень умеренная.',false),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c',NULL,NULL,NULL,'00:15:00',NULL,'Кедровые орехи; Миндаль можно частично или полностью заменить на кедровый, или другой орех по вашему вкусу Количество жидкости (маринада или воды) добавляйте, ориентируясь на консистенцию и потребности блендера; Пробуйте и корректируйте рецепт под себя. Пропорции и составляющие можно менять, что-то убирать или добавлять, но сначала рекомендую попробовать приготовить по рецепту ---',NULL,'дополнение к овощам и зелени; заправка для салатов; соус для пасты; дополнение к запечённой картошке и другим блюдам; соус для лепёшек, тостов, блинов','Песто с руколой и оливками','орехи,жиры','Необычное, слегка пикантное сочетание, которое непременно вызовет яркие эмоции и подойдёт к разным блюдам',false),
	 ('4b04bcba-f0ec-5443-b03a-fafc09b4c737',NULL,NULL,NULL,'00:55:00',NULL,NULL,'хранить в морозилке до 3-6 месяцев',NULL,'Яблочное пюре','Яблочное пюре','Домашнее пюре из запечённых яблок, которое можно использовать как для кексов, бисквитов, так и для рецептов зефира или других десертов. Или даже как самостоятельное блюдо.; --- ⏰ Общее время: 40-55 минут 👩🏻‍🍳 Активное время: 10-15 минут ⏳ Ожидание: 30-40 минут ---',false),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',NULL,NULL,NULL,'00:45:00',NULL,'Овощи,Кокосовый сахар; - Для начинки можно использовать любые овощи по вкусу, а также отварную фасоль / чечевицу / нут - Вместо твёрдого сыра можно использовать любой соус по вашему желанию (например, кешью-соус)',NULL,NULL,'Кесадилья','жиры,бобы','Хрустящая нутовая лепёшка с сочной начинкой с домашним сыром. К ней подходит любая начинка, поэтому смело экспериментируйте',false),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',NULL,NULL,NULL,'01:00:00',NULL,'Растительное масло,Растительное молоко,Сироп топинамбура,Сироп кленовый; ~ на 2 пиццы d 25 см или на 16-20 пирожков; - Миндальную муку можно сделать самостоятельно, измельчив миндаль в кофемолке и просеяв через мелкое сито - Количество воды зависит от впитываемости муки и псиллиума, её может понадобиться больше, ориентируйтесь на консистенцию - Для большей мягкости и вкуса в тесто можно добавить 30 г растительного масла или часть воды заменить на растительное молоко. А также добавить 2 ст.л. сиропа топинамб','хранить в морозилке, затем разморозить и запечь',NULL,'Тесто для пиццы, пирожков, булочек','бобы,крупы,жиры,орехи,псиллиум,крахмал','Универсальное тесто, мягкое и воздушное, простое в приготовлении, без закваски. Подходит для хлеба, пирожков, булочек, пиццы и пирогов',false),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f',NULL,NULL,NULL,'00:10:00',NULL,'Пропорции можно менять: больше хлопьев - для более густых и жирных сливок или, наоборот, меньше - для более жидкого и менее жирного молока',NULL,NULL,'Овсяные сливки','крупы',NULL,false),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',NULL,NULL,NULL,'00:35:00',NULL,'на 1 большую лепёшку на сковороду 28 см; - Кабачок можно не добавлять. Просто исключите его из рецепта и смешайте тесто с сыром - Также можно добавлять другие овощи, например, помидоры, перец, грибы, брокколи. В сыром виде, или же предварительно приготовленные. В первом случае готовить лепёшку нужно дольше, чтобы овощи пропеклись. Во втором можно сократить время жарки - Состав муки можно подбирать на свой вкус. Например, вместо овсяной можно взять нутовую',NULL,NULL,'Сырная лепёшка (ленивый хачапури)','жиры,крупы,кокос','Лепёшка с зажаристой хрустящей корочкой и сочной тягучей начинкой, похожа на хачапури. В рецепте привожу вариант с добавлением тёртого кабачка, можно его не добавлять или экспериментировать с другими добавками.',false),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',NULL,NULL,NULL,'12:00:00',NULL,'Соль,Специи,Зелень; выход из данного количества - 350-500 г сметаны; - Кокосовое молоко должно быть обязательно жирное, но жидкое. Самый подходящий вариант - 17-19%. Этот рецепт рассчитан именно на кокосовое молоко, заменить его нельзя - Выбирайте однородное и наиболее нейтральное по вкусу молоко. Я рекомендую бренд Vico - Количество лимонного сока может составлять 5-10% от объёма молока и зависит от того, насколько кислую сметану вы хотите. Для десертов, например, больше подойдёт сметана с содержанием сока','Хранить готовую сметану в холодильнике в закрытой ёмкости до 3-5 суток','соус к блинам, вареникам и другим блюдам; заправка для салатов; запекать с ней картошку и другие овощи; сметанный крем для десертов: медовик, морковный торт, тирамису, блинный торт, панчо и пр.','Сметана кокосовая','кокос,жиры','Этот вариант сметаны на мой вкус получается максимально близким к традиционной сметане, и по вкусу, и по текстуре. Благодаря процессу приготовления можно адаптировать плотность и кислоту под себя, что позволяет использовать её для разных рецептов - как для десертов, так и для несладких блюд. Также особенность этой сметаны в том, что она взбивается в пышную массу, благодаря чему подходит для воздушных и устойчивых кремов',false),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',NULL,NULL,NULL,'01:00:00',NULL,'Можно запечь тыкву целиком или кусочками, выбирайте наиболее удобный вам вариант; - Удобно заготовить сразу много и хранить в морозилке','хранить в морозилке','Выпечка, например: Тыквенный хлебили Панкейки тыквенные; Соусы; Десерты','Тыквенное пюре','Тыквенное пюре','Для рецептов, в которых используется тыквенное пюре.',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd',NULL,NULL,NULL,'01:10:00',NULL,'Имбирь; - По желанию можно добавить корень имбиря (свежий или сушёный в виде порошка) для остроты и большего согревающего эффекта - Кокосовое молоко придаёт шелковистую текстуру и слегка сливочный вкус. Но вы можете заменить его водой, овощным бульоном или же другим растительным молоком #### Приготовление 1. Тыкву нарезать ломтиками вместе с кожурой. Перец и лук нарезать крупно. 2. Головку чеснока помыть и срезать у неё верхушку, так, чтобы оголились зубчики. 3. Выложить все овощи и яблоко на антипригарный ',NULL,NULL,'Тыквенный крем-суп','жиры','Самый насыщенный рецепт тыквенного супа, который я делала! Благодаря такому сочетанию ингредиентов и запеканию овощей он получается очень вкусный и сбалансированный, а не просто сладкий. Но вы всегда можете адаптировать состав под себя',false),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd',NULL,NULL,NULL,'01:10:00',NULL,'- Пропорции можно менять: больше миндаля - для более жирного насыщенного молока, или, наоборот, меньше - для более лёгкого молока - Также для вкуса при взбивании можно добавить финик/сироп, корицу','хранить в холодильнике до 4-5 дней',NULL,'Миндальное молоко','жиры,орехи',NULL,false),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',NULL,NULL,NULL,'00:07:00',NULL,'на платформе есть также ягодный вариант тирамису - Тирамису клубничный / торт Фрезье --- ⏰ Общее время: 7+ часов 👩🏻‍🍳 Активное время: 1 час - 1 час 30 минут ⏳ Ожидание: 6+ часов (охлаждение, пропитка) --- #### Инвентарь - Миксер подойдёт как планетарный, так и ручной (мощностью от 300 Ватт) - Мешочек для молока или марля + сито/дуршлаг для отвешивания сметаны - Пергамент силиконизированный для выпечки - Кондитерские мешки (необязательно) Для отсаживания печенья в виде савоярди. Можно сделать это с помощью о','хранить в холодильнике закрытым до 3 суток',NULL,'Тирамису','жиры,орехи,кокос,крупы,подсластитель','Нежный тирамису с воздушным сметанным кремом без кофе, но с карамельно-кофейными нотами. Если вы хотите максимально приближенный вкус к оригинальному тирамису, для пропитки бисквитов используйте эспрессо, а для посыпки - какао-порошок',false),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',NULL,NULL,NULL,'00:07:00',NULL,'- Кокосовое молоко можно заменить на любое другое растительное нежирное молоко. С кокосовым крем будет более светлым и сливочным - Сироп не рекомендую заменять на кокосовый/тростниковый сахар, так как это сделает крем более тёмным и с карамельным привкусом - Ваниль - подойдёт ванильная паста, ванильный экстракт, порошок сушёной ванили или семена из свежего стручка ванили. Я использую ванильную пасту ~ 1 ч.л. - Картофельный крахмал в данном случае возможно заменить на тапиоковый, его потребуется меньше, прим','хранить в холодильнике закрытым до 3 суток',NULL,'Тирамису клубничный / торт Фрезье','жиры,орехи,кокос,крупы,подсластитель,крахмал','Нежнейший десерт с воздушным кремом и освежающим вкусом ягод. Это клубничная интерпретация классического рецептаТирамису',false),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',NULL,NULL,NULL,'00:03:00',NULL,'Можно готовить в виде торта или роллов --- ⏰ Общее время: 3+ часов (если готовы сметана и сгущёнка) 👩🏻‍🍳 Активное время: 50 минут - 1 час 10 минут ⏳ Ожидание: 2+ часов (охлаждение, пропитка) --- #### Инвентарь - Миксер подойдёт ручной (мощностью от 300 Ватт), в т.ч. блендер с насадкой венчик - Мешочек для молока или марля + сито/дуршлаг для отвешивания сметаны - Пергамент или плёнка - Сковорода антипригарная блинная или обычная ---; Для начала необходимо заранее подготовить сметану и сгущёнку для крема. Удо',NULL,NULL,'Блинный рулет карамельный с кэробом, бананом и орехами','крупы,крахмал,жиры,кокос,подсластитель','Мягкий и нежный рулет из "шоколадных" блинов с карамельным кремом. Идеальное сочетание воздушного крема на основе взбитой кокосовой сметаны и варёной сгущёнки, блинов с кэробом, бананом и орехами. Приготовьте этот нестандартный блинный десерт на Масленицу или любой другой праздник, и все гарантированно будут восхищены!',false),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae',NULL,NULL,NULL,'00:02:00',NULL,'- Кокосовое молоко такой жирности необходимо для правильного уваривания и однородной густой текстуры сгущёнки. Заменять не рекомендуется - Кокосовый сахар можно заменить на тростниковый или на сироп (топинамбура/кленовый). С сиропом топинамбура будет светлая и нейтральная по вкусу сгущёнка, и не будет засахариваться со временем. Например, для крема с карамельным вкусом лучше использовать сгущёнку на кокосовом сахаре - Вы также можете смешать кокосовый сахар и сироп (например, 1:1), если хотите нечто среднее','хранить до 3 месяцев в морозилке',NULL,'Варёная сгущёнка','жиры,кокос,подсластитель','Самый простой вариант сгущёнки из 2 ингредиентов, густоту которой вы можете регулировать сами. Можно использовать как самостоятельную добавку к сладким блюдам: блинам, панкейкам, сырникам. Так и в составе десертов, например, для крема.',false),
	 ('f0330825-3177-5e8e-9956-796217d9dc47',NULL,NULL,NULL,'01:20:00',NULL,'- похожий рецепт без псиллиума с семенами льна: Блины с семенами льна --- ⏰ Общее время: 1 час - 1 час 20 минут 👩🏻‍🍳 Активное время: 30-40 минут ⏳ Ожидание: 30-40 минут ---; примерно на 15 блинов (на сковороде 26 см); - Молоко можно использовать любое нежирное (например, жирное кокосовое разбавить водой, или приготовить домашнее ореховое). Полностью заменять на воду не рекомендую, так как молоко придаёт блинам мягкость, румяность и вкус. При недостатке жиров блины могут трескаться и быть более резиновыми - ',NULL,NULL,'Блины с псиллиумом','жиры,псиллиум,крупы,крахмал','Эти блины больше всего напоминают мне классические: они эластичные, мягкие и нежные, в меру тонкие. Подходят как для сладкой, так и несладкой версии.',false),
	 ('7e931153-f384-589b-92be-b6cb7455a603',NULL,NULL,NULL,'01:10:00',NULL,'- к этим блинам хорошо подойдёт: Варёная сгущёнкана кокосовом сахаре - используются в рецепте: Блинный рулет карамельный с кэробом, бананом и орехами --- ⏰ Общее время: 50 минут - 1 час 10 минут 👩🏻‍🍳 Активное время: 20-30 минут ⏳ Ожидание: 30-40 минут ---; примерно на 10 блинов (на сковороде 26 см), количество можно менять под себя; - Молоко можно использовать любое нежирное (например, жирное кокосовое разбавить водой). Полностью заменять на воду не рекомендую, так как молоко придаёт блинам эластичность и м',NULL,NULL,'Блины "шоколадные"','крупы,крахмал,жиры,подсластитель','Эластичные блинчики с кэробом, который даёт лёгкий шоколадно-пряный вкус. Подходят для сладких топпингов и начинок, а также тортов и рулетов.',false),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',NULL,NULL,NULL,'00:05:00',NULL,'- я привожу 2 варианта смесей, на базе которых вы можете экспериментировать, менять пропорции, убирать или заменять муку на другую - также вы можете готовить блины на основе только киноа --- ⏰ Общее время: 2,5+ часа (с замачиванием киноа) 👩🏻‍🍳 Активное время: 25-30 минут ⏳ Ожидание: 2+ часа (замачивание киноа) --- 1 вариант (с рисовой и миндальной мукой - ореховый аромат и вкус хорошо перекрывает киноа, жиры из миндаля придают мягкость); - Молоко можно использовать любое нежирное (например, жирное кокосовое',NULL,NULL,'Блины из киноа','крупы','Киноа - крупа с высоким содержанием белка, имеет склеивающие свойства, поэтому из неё можно сделать тесто для блинов даже без добавления других компонентов.',false),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',NULL,NULL,NULL,'01:00:00',NULL,'- похожий рецепт без семян льна с псиллиумом: Блины с псиллиумом --- ⏰ Общее время: 50 минут - 1 час 👩🏻‍🍳 Активное время: 30-40 минут ⏳ Ожидание: 15-20 минут ---; - Молоко можно использовать любое нежирное (например, жирное кокосовое разбавить водой или приготовить домашнее ореховое). Полностью заменять на воду не рекомендую, так как молоко придаёт блинам эластичность и мягкость. При недостатке жиров блины могут трескаться и быть более резиновыми - Количество сиропа регулируйте по своему вкусу, или можно ег',NULL,NULL,'Блины с семенами льна','жиры,крупы,крахмал','Эластичные, мягкие и нежные блинчики красивого золотистого цвета. Подходят как для сладкой, так и несладкой версии.',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',NULL,NULL,NULL,'01:20:00',NULL,'- Блины подойдут любые из несладкого теста. В тесто можно не добавлять подсластитель или добавить его минимально для баланса вкусов, а также добавить соль по вкусу - Творожный сыр (рикотта) подойдёт более мягкий, то есть достаточно его отвесить без пресса - Кокосовое молоко используется для соуса "бешамель". Для более сливочного вкуса рекомендую брать кокосовое молоко 17-19%, но если вы хотите снизить жирность, то можно брать менее жирное - Картофель нужен для загущения и облегчения соуса. Если вы не хотите',NULL,NULL,'Каннеллони с рикоттой и шпинатом','жиры,орехи,крупы','Каннеллони - итальянская паста в виде трубочек, которые фаршируются начинкой (в классическом варианте чаще всего фаршем или сыром), заливаются соусом и запекаются в духовке.',false),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',NULL,NULL,NULL,'00:50:00',NULL,'--- ⏰ Общее время: 35-50 минут 👩🏻‍🍳 Активное время: 25-35 минут ⏳ Ожидание: 10-15 минут ---; При желании вы можете сделать блины безжировыми, заменив молоко на воду. Но имейте в виду, что с молоком вкус будет более насыщенным, а блины - мягче',NULL,NULL,'Блины с яблоком','крупы,бобы,подсластитель,крахмал','Это не обычные блины, а блины с добавлением яблока в тесто. Они получаются сочные, мягкие и ароматные, при этом эластичные.',false),
	 ('fcb65d25-0886-5071-a188-33d37d560668',NULL,NULL,NULL,'01:00:00',NULL,'- Блины подойдут любые из несладкого теста. В тесто можно не добавлять подсластитель или добавить его минимально для баланса вкусов, а также добавить соль по вкусу. Для "мешочков" нужны более эластичные блинчики, как, например, Блины с псиллиумомили Блины с семенами льна - Начинка "жюльен" используется в незапечённом виде, после прогревания соуса с грибами на сковороде - Сыр расплавится при нагревании (если вы готовите блины на сковороде или в духовке) и придаст ощущение слегка тягучей текстуры. Поэтому его',NULL,NULL,'Блины "жюльен"','жиры,крупы,кокос','Блинчики с сочной сливочно-грибной начинкой. Можно обжарить на сковороде для хрустящей корочки или просто завернуть. Для более эффектной праздничной подачи',false),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',NULL,NULL,NULL,'00:09:00',NULL,'Регулируя текстуру и вкус для разных задач, вы можете получить как мягкий сыр, рикотту, так и более плотный творог; Выход творога из данного количества ~ 300-350 г; - Соотношение миндаля к кешью можно менять. От 4:1 до 1:1 (миндаль : кешью) - Кешью можно полностью заменить на миндаль. Но не наоборот, так как именно миндаль даёт нужную текстуру творогу. При этом кешью добавляет больше сливочности и смягчает ореховый вкус, поэтому я добавила его в рецепт Пробуйте разные варианты и выбирайте свой идеальный','хранить в холодильнике до 5 дней','для намазок на хлеб/лепёшки/блины (в видео показан вариант творожного сыра с зеленью); для несладких блюд (например, используется в каннеллони с рикоттой и шпинатом); для десертов и выпечки как обычный творог (запеканки, чизкейки, сырники и т.д.); в качестве самостоятельного блюда (с ягодами, фруктами, гранолой); Мешочек для орехового молока; Сито и миска, между которыми есть пространство, если сито положить на миску; Марля (при необходимости); Пресс в виде контейнера, банки или миски с водой','Творожный сыр','орехи,жиры','Этот ореховый сыр ещё можно назвать рикоттой или творогом. Имеет неоднородную творожную текстуру благодаря технологии приготовления, и по вкусу действительно напоминает творог',false),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',NULL,NULL,NULL,'01:10:00',NULL,'Можно делать порционно - в специальных кокотницах, керамических или силиконовых формочках для маффинов. Или запечь в одной большой форме; - Грибы можно брать любые по вашему вкусу. Вешенки дадут более волокнистую структуру. Можно сделать смесь шампиньонов и вешенок - Картофель можно отварить в воде, но в таком случае может потребоваться меньше жидкости - Кокосовое молоко придаёт сливочность. Жирность подбирайте по своему вкусу. Я беру жирное 17-19% и немного разбавляю водой. Возможны замены на другое растит',NULL,NULL,'Жюльен грибной','жиры','Это здоровая облегчённая версия традиционного жюльена, но не менее насыщенная. В нём сохраняется сливочность и запечённая корочка. Очень простое блюдо, которое при этом легко станет частью праздничного стола.',false),
	 ('da9c1cec-6648-54ac-9977-5658237b1608',NULL,NULL,NULL,'00:25:00',NULL,'- Вместо овсянки и пшена можно взять уже готовую муку в том же объёме - Если тыквенное пюре у вас достаточно водянистое, то дополнительная вода может не понадобиться - Количество сиропа регулируйте по своему вкусу - По желанию можно добавить в тесто пряности для более согревающего осеннего вкуса',NULL,NULL,'Панкейки тыквенные','крупы,подсластитель','Это полностью безжировые, пушистые и ароматные панкейки на основе тыквенного пюре, овсяных хлопьев и пшена. У них хлебная текстура с румяной корочкой, а тыквенное пюре делает мякиш более нежным и бархатистым.',false),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',NULL,NULL,NULL,'00:05:00',NULL,'на 1 форму 12-13 см или на 2 поменьше (~9 см); - Миндальную муку можно смолоть из миндаля самостоятельно в кофемолке или блендере и просеять через сито - Пшённую муку можно смолоть из пшена самостоятельно в кофемолке или блендере - Нутовую муку можно заменить на смесь 30 г миндальной и 30 г пшённой муки - Растительное молоко можно использовать кокосовое нежирное или любое ореховое - Масло виноградной косточки можно заменить на другое нейтральное жидкое растительное масло (горчичное, авокадо). Масло делает к','хранить в морозилке до 2 месяцев',NULL,'Кулич','жиры,орехи,псиллиум,крупы,подсластитель','Это рецепт ароматного кекса, который идеально подходит для куличей. Он хорошо пропекается внутри даже в больших формах, получается пористым, сочным и мягким. Можно печь и в формах для кекса или капкейков по любым поводам или просто к чаю. Также в этом рецепте будет несколько вариантов шапочки для кулича.',false),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',NULL,NULL,NULL,'08:00:00',NULL,'- Рецепт рассчитан на форму для 900 г массы. Пасха довольно сытная, поэтому при необходимости пересчитывайте под себя - После заморозки текстура пасхи получается более плотной и творожной; Выход ~ 900 г; - Вес кешью указан уже в замоченном виде. Сухого ореха понадобится примерно 170 г - Пшено отварить в пропорции 1:3 с водой так, чтобы оно было плотное и сухое, без жидкости - Сухофрукты и орехи для начинки предварительно промыть и хорошо просушить. Орехи лучше всего обжарить в духовке или на сковороде для р','хранить в холодильнике до 5 дней','По этому рецепту можно делать творожные сырки, заливая массу в соответствующие формы; Использовать как творожную массу для начинки, например, в блины; Форма для творожной пасхи (Пасочница) - количество ингредиентов рассчитано на форму на 1 кг массы; Марля (не обязательно)','Пасха из кешью и пшена','орехи,жиры,крупы,подсластитель,кокос','«Творожная» пасха, простая в приготовлении и с простым составом. В основе - кешью, пшено и кокосовый урбеч. В таком варианте пасхи кешью дает сливочность, а пшено - дополнительную текстуру и плотность, благодаря чему снижает жирность.',false),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',NULL,NULL,NULL,'00:08:00',NULL,'- Альтернативный рецепт пасхи - Пасха из кешью и пшена - В рецепте нет никаких загустителей и дополнительных жиров, поэтому пасха довольно нежная, кремовая, но при этом держит форму. Попробуйте приготовить, и, если вы захотите более плотную текстуру, можно добавить кокосовое масло или урбеч; Выход ~ 500 г; - Кешью можно полностью заменить на миндаль - Можно использовать готовый плотный Творожный сыр. Просто смешайте его с сахаром, пробейте блендером, вмешайте вкусовые добавки, выложите в форму и поставьте п','хранить в холодильнике до 5 дней','По этому рецепту можно делать творожные сырки, заливая массу в соответствующие формы; Использовать как творожную массу для начинки, например, в блины; Форма для творожной пасхи (Пасочница) - количество ингредиентов рассчитано на форму на 500 г массы; Марля (не обязательно); Сито или решётка; Пресс (банка/миска с водой); Мешочек для орехового молока','Пасха ореховая','орехи,жиры,подсластитель','Этот вариант пасхи, на мой взгляд, имеет самые приближенные к творожным текстуру и вкус. Готовится она из орехов по принципу рецепта: Творожный сыр',false),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',NULL,NULL,NULL,NULL,NULL,NULL,'хранить в холодильнике до 3 суток под плёнкой или в контейнере, без доступа воздуха',NULL,'Пельмени без глютена','Пельмени без глютена','Выход ~ 30-35 штук',false);
INSERT INTO public."Recipes" ("Id","BaseRecipe","PhotoId","VideoId","CookingTime","CookingComment","IngredientComment","StorageComment","UsageComment","Name","Description","Comment","IsDeleted") VALUES
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',NULL,NULL,NULL,'01:25:00',NULL,'- Идеальное дополнение к картофельным блинам - Сметана кокосоваяили Твёрдый сыр - Эти блины также можно готовить без припёка - Для сладкой версии (например, с яблоком) добавьте в тесто подсластитель и уберите соль - Похожий рецепт с яблоком - Блины с яблоком --- ⏰ Общее время: 1 час - 1 час 25 минут 👩🏻‍🍳 Активное время: 30-45 минут ⏳ Ожидание: 30-40 минут ---; - Молоко придаёт блинам румяность и мягкость, позволяет лучше отходить от сковороды. Полностью на воду заменять не рекомендую, но вы можете регулиров','хранить в холодильнике до 3 дней',NULL,'Блины с картошкой и зеленью','крупы,бобы','Блины «с припёком», ароматные, уютные и сытные. Этот рецепт теста хорошо подходит, чтобы замешивать в него добавки. Можно таким образом добавлять не только картошку, но и грибы, лук, яблоко, тыкву.',false),
	 ('1755d74c-998a-545b-b725-0439d99037fb',NULL,NULL,NULL,'01:00:00',NULL,'- Вместо гречневой муки можно использовать цельную зелёную гречку (80 г). Для этого замочите её в холодной воде на ночь, затем хорошо промойте, положите в блендер вместе с молоком (его в таком случае потребуется меньше, начните с 450 г) и остальными ингредиентами и взбейте до однородной массы - Овсяную муку можно измельчить самостоятельно в кофемолке или блендере. Если вы используете цельную гречку, то так же можно замочить и овсяные хлопья и взбить вместе с гречкой - Рисовую муку можно использовать как бел','Хранить можно в холодильнике до 3 дней в закрытом контейнере',NULL,'Блины из зелёной гречки','крупы','В этих блинах основной компонент - мука зелёной гречки. Она хорошо связывает, но даёт жёсткость и сильный привкус, поэтому я рекомендую использовать её в комбинации с другими видами муки. Здесь привожу один такой вариант, но вы можете экспериментировать и миксовать гречку с разной мукой или использовать её в моно формате.',false);
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','23773894-cdfb-5005-b5a6-0c76a5bf02f6',true,'Овсяные хлопья','Овсяные хлопья',NULL,'2:стакан;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','13900dce-3e69-555f-b088-0de3c5191a04',true,'Киноа','Киноа',NULL,'0.5:стакан;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'3:ст.л.;1:по вкусу;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','для оттенения сладости','1:щепотка;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','53c35600-d58b-51bb-b65b-8998aab99d22',true,'Паста ореховая','Паста ореховая','фундук, миндаль, кешью, подсолнечник, кокосовая паста и т.д.','2:ст.л.;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','97beb515-943d-5a8b-9100-2f7a2471ba87',true,'Орехи','Орехи',NULL,'1:по вкусу;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','080247c9-68d3-5101-bb73-c46ae33f9172',true,'Кокосовая стружка','Кокосовая стружка',NULL,'1:по вкусу;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','e01d8bf5-b6bc-54c3-a3d7-9d6ddc746508',true,'Воздушный амарант','Воздушный амарант',NULL,'1:по вкусу;'),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63','f4cd40bd-ce2a-546d-890e-251275ad1828',true,'Фруктовое','Фруктовое','яблочное, банановое, тыквенное','1:ст.л.;'),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e','a5593de5-0438-5a7d-9c70-cd49195facf4',true,'Семена амаранта','Семена амаранта','или киноа','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','6ecb4c22-d683-525f-9bd5-1a4564818887',true,'Томатная паста','Томатная паста','Раздел: Барбекю соус','100:г;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','1979006a-d7d7-505f-a029-4dea97dff18c',true,'Апельсиновый сок','Апельсиновый сок','Раздел: Барбекю соус','4:ст.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре','Раздел: Барбекю соус','2:ст.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','389cb4b0-c501-5f31-a2ca-9c0c5d1d2f8d',true,'Паприка копченая','Паприка копченая','натурального копчения; Раздел: Барбекю соус','0.5:ч.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','Раздел: Барбекю соус','0.5:ч.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Чеснок сушеный','Раздел: Барбекю соус','0.5:ч.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','6a4daef6-0e4c-5b49-ac40-c882869fbf13',true,'Семена горчицы','Семена горчицы','Раздел: Барбекю соус','1:по вкусу;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','95258fee-371a-561b-9762-92f831d44a36',true,'Фасоль красная','Фасоль красная','Раздел: Для котлет','350:г;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','3a1cd39e-cec7-543b-9759-bde7daec371d',true,'Пшено','Пшено','Раздел: Для котлет','250:г;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель','Раздел: Для котлет','300:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук','Раздел: Для котлет','1:шт.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок','Раздел: Для котлет','2:шт.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы','Раздел: Для котлет','100:г;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','fc025614-2325-583a-96de-e0414ca683f6',true,'Зира','Зира','кумин; Раздел: Для котлет','0.5:ч.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','430094cf-2d4a-5e17-b18c-5b2edde4c715',true,'Соус барбекю','Соус барбекю','Раздел: Для котлет','2:ст.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','28f6f887-a6bc-5516-8224-9d3e1ab0d1fd',true,'Кокосовые аминокислоты','Кокосовые аминокислоты','Раздел: Для котлет','2:ст.л.;'),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b','7f9c1e58-9d54-5561-a922-c075d743c9d9',true,'Кинза','Кинза','Раздел: Для котлет','1:по вкусу;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','8eb1095b-6eac-5c60-baa7-53e95bf88beb',true,'Сырный соус из картошки','Сырный соус из картошки',NULL,'1:шт.;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','23c3e842-e0e2-5e15-887f-c38f188a2fe7',true,'Чечевица красная','Чечевица красная',NULL,'1:стакан;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'3:шт.;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',true,'Помидор','Помидор',NULL,'300:г;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','6ecb4c22-d683-525f-9bd5-1a4564818887',true,'Томатная паста','Томатная паста',NULL,'3:ст.л.;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:ч.л.;1:по вкусу;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','22e69402-41ae-5cd0-87ec-e68447cc11e5',true,'Орегано','Орегано',NULL,'1:ч.л.;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','02922fee-9ec2-56b8-8491-3d36eaca7335',true,'Перец белый','Перец белый',NULL,'0.25:ч.л.;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:стакан;'),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5','3ef92a25-a13a-5092-ab2e-ca888882145c',true,'Кабачок','Кабачок',NULL,'2:шт.;'),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3','83918035-d6e2-5d58-9f5f-e75b7aee1fa9',true,'Мука пшенная','Мука пшенная',NULL,'2:стакан;'),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'2:стакан;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;1:по вкусу;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','e9beccc4-222a-5078-9b90-f6cd1360bd67',true,'Мука чечевицы','Мука чечевицы',NULL,'0.5:стакан;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','d6a1eeb1-b75b-5365-ac54-dbac72700a31',true,'Мука зеленой гречки','Мука зеленой гречки',NULL,'0.25:стакан;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','83918035-d6e2-5d58-9f5f-e75b7aee1fa9',true,'Мука пшенная','Мука пшенная',NULL,'0.25:стакан;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','ee3cce66-8244-5982-ad5d-a8b5eaedac23',true,'Семена льна','Семена льна',NULL,'2:ст.л.;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал',NULL,'3:ст.л.;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','a111617d-d2b4-51d9-adf5-8d74d7a85a41',true,'Тыквенное пюре','Тыквенное пюре',NULL,'1:стакан;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:по вкусу;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'1:ч.л.;'),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','0ac426d9-0a4f-569c-89f3-e36e89d6c036',true,'Зелёный горошек','Зелёный горошек',NULL,'1:по вкусу;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:шт.;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','ecf08b78-17c0-503b-9ef7-26108fb75e91',true,'Петрушка','Петрушка',NULL,'1:пучок;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','059b624a-58b3-57d4-b265-60297c25b705',true,'Кумин','Кумин','зира','0.5:ч.л.;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'4:ст.л.;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'3:ст.л.;'),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель','Раздел: Вариант 2','3:шт.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь',NULL,'1:шт.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('98638408-7fec-598e-90a3-eba036fa54df','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'0.5:ст.л.;1:по вкусу;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Чеснок сушеный',NULL,'2:ч.л.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','9874594b-71a3-50cc-b314-09e52b719724',true,'Мускатный орех','Мускатный орех',NULL,'1:щепотка;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','c2144e2f-7e15-5b03-bb98-5a2c88e1ad9a',true,'Паприка','Паприка',NULL,'0.5:ч.л.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','404a9c1f-8fba-5ee7-b828-40256eb53aab',true,'Базилик сушеный','Базилик сушеный',NULL,'1:ч.л.;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','3ee68412-100f-5fae-a89a-a416fa5f1beb',true,'Цветная капуста','Цветная капуста','Раздел: Вариант 2','1:по вкусу;'),
	 ('98638408-7fec-598e-90a3-eba036fa54df','13b4b933-fc3a-50e0-b7e6-98e7846bdc5a',true,'Лук сушеный','Лук сушеный','Раздел: Вариант 2','1:ч.л.;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'150:г;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'50:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','0f4e75e9-6d57-5882-bdb7-13668f79ea15',true,'Крахмал тапиоки','Крахмал тапиоки',NULL,'3:ст.л.;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'1.5:ч.л.;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','da004df1-ec9f-58cd-a1b6-7c4f272fce7a',true,'Молоко растительное','Молоко растительное','так как в рецепте нет масла, лучше брать более жирное, у меня кокосовое','200:г;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре','готовое или самодельное из запечённых зелёных яблок','100:г;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'70:г;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','c2af850e-9593-5627-acc6-92eed80dd7eb',true,'Яблоко','Яблоко',NULL,'1:по вкусу;'),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b','f686acf2-e4eb-5a42-8406-eec1f40b66bb',true,'Корица','Корица',NULL,'1:по вкусу;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука','Раздел: Для теста','200:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука','Раздел: Для теста','50:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','5810a4fc-11b8-52ab-b0ea-964b8cdd922f',true,'Крахмал','Крахмал','Раздел: Для теста','50:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','bdb1f62b-9795-5f76-93ed-d821c53847af',true,'Кокосовое масло','Кокосовое масло','Раздел: Для теста','50:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','a9b0e13a-3138-5a17-a37b-e09bed645d1e',true,'Сахар панела','Сахар панела','Раздел: Для теста','40:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре','Раздел: Для теста; Раздел: Чизкейк','100:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','Раздел: Для теста','1:щепотка;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода','Раздел: Для теста','30:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый','Раздел: Карамельные яблоки','50:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','Раздел: Карамельные яблоки','50:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','c2af850e-9593-5627-acc6-92eed80dd7eb',true,'Яблоко','Яблоко','Раздел: Карамельные яблоки','300:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Карамельные яблоки','1:ст.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','f686acf2-e4eb-5a42-8406-eec1f40b66bb',true,'Корица','Корица','Раздел: Карамельные яблоки','1:ч.л.;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','991a021e-f869-533e-9aca-6ce7cb2a56f6',true,'Кешью','Кешью','сухой; Раздел: Чизкейк','200:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','3dcb7477-4d6c-5511-a5bd-0e87d40c7258',true,'Кокосовые сливки','Кокосовые сливки','Раздел: Чизкейк','100:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал','Раздел: Чизкейк','3:ст.л.;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Чизкейк','70:г;'),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','Раздел: Чизкейк','1:по вкусу;'),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','080247c9-68d3-5101-bb73-c46ae33f9172',true,'Кокосовая стружка','Кокосовая стружка','чем она жирнее и свежее, тем вкуснее','1:стакан;'),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','жирное, от 17%','1:стакан;'),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','bdad705e-0503-5cd1-ae9a-1a7c2b042e1b',true,'Манго','Манго','Раздел: Для глазури','1:шт.;'),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','71b5608c-304b-58f9-b5a8-51d8c4bfcb02',true,'Агар-агар','Агар-агар',NULL,'1:ч.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'3:ст.л.;'),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216','bf5422ac-1f24-5072-a884-a95f187f4125',true,'Кокосовый урбеч','Кокосовый урбеч','паста','3:ст.л.;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','bdad705e-0503-5cd1-ae9a-1a7c2b042e1b',true,'Манго','Манго',NULL,'1:по вкусу;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','af17f6a2-686a-5f33-bbcb-7eca5f7e8253',true,'Кокосовая паста','Кокосовая паста','урбеч','1:по вкусу;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','71b5608c-304b-58f9-b5a8-51d8c4bfcb02',true,'Агар-агар','Агар-агар',NULL,'0.5:ч.л.;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'2:ст.л.;1:по вкусу;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','01ae364e-e413-58aa-bd32-fe30202262ce',true,'Миндаль жареный в начинку','Миндаль жареный в начинку',NULL,'1:по вкусу;'),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8','080247c9-68d3-5101-bb73-c46ae33f9172',true,'Кокосовая стружка','Кокосовая стружка для обсыпки',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'0.5:шт.;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','ff2ccb17-41b5-5d43-b248-96ff694e30a0',true,'Имбирь','Имбирь',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','b07f8f1d-df35-58c0-b3a0-dc827192505f',true,'Пекинская капуста','Пекинская капуста',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь',NULL,'1:шт.;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','c28bad38-775f-51c8-9d09-877e3b039384',true,'Вешенки','Вешенки',NULL,'200:г;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','c8403fed-a2c7-5fe7-b6bf-7abc0318a2df',true,'Лук зелёный','Лук зелёный',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','f8715b4e-305d-5318-953f-ac8cc71d77c1',true,'Соль по вкусу','Соль по вкусу',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','69e5987e-6cbd-5583-b42d-d2f86710fe22',true,'Специи','Специи',NULL,'1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','0358f024-c462-55a5-8295-63b999c64af6',true,'Готовая фасоль','Готовая фасоль',NULL,'1:стакан;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','202b5aa5-f84b-5069-925d-75e1d4689953',true,'Листы нори','Листы нори','сушёные без масла, нарезать на небольшие прямоугольники','1:по вкусу;'),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','460c3eb1-42a4-51f8-9f19-adc6c7c31d50',true,'Кунжут','Кунжут',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215','40fcbd45-bae4-54f3-8d9c-595d86745f43',true,'Рисовая бумага','Рисовая бумага',NULL,'1:по вкусу;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','28f6f887-a6bc-5516-8224-9d3e1ab0d1fd',true,'Кокосовые аминокислоты','Кокосовые аминокислоты',NULL,'1:по вкусу;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:по вкусу;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'0.5:ст.л.;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','ff2ccb17-41b5-5d43-b248-96ff694e30a0',true,'Имбирь','Имбирь',NULL,'1:по вкусу;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;'),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал',NULL,'0.5:ст.л.;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','991a021e-f869-533e-9aca-6ce7cb2a56f6',true,'Кешью','Кешью',NULL,'300:г;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','85759161-d1a1-5eec-af9b-1d341026533d',true,'Нут','Нут',NULL,'170:г;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','a30ae4ef-07f9-502f-ad14-c1ed5ca5042b',true,'Цедра лимона','Цедра лимона',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал',NULL,'2:ст.л.;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'80:г;1:по вкусу;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'300:г;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','у меня порошок из сушёных стручков','1:по вкусу;'),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a','a62b2882-7b48-5fb7-9fdc-86c21a327fba',true,'Черная смородина','Черная смородина',NULL,'100:г;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','0020c500-2165-5b16-aba8-775ab26990bd',true,'Баклажан','Баклажан',NULL,'4:шт.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь',NULL,'2:шт.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',true,'Перец сладкий','Перец сладкий',NULL,'1:шт.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'4:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','c2144e2f-7e15-5b03-bb98-5a2c88e1ad9a',true,'Паприка','Паприка',NULL,'1:ч.л.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','059b624a-58b3-57d4-b265-60297c25b705',true,'Кумин','Кумин',NULL,'0.5:ч.л.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:ч.л.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','a72af268-3dd8-58d1-be0b-42bcfd48ef6b',true,'Протертые томаты','Протертые томаты',NULL,'400:г;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','22e69402-41ae-5cd0-87ec-e68447cc11e5',true,'Орегано','Орегано',NULL,'1:ч.л.;'),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736','1c73179c-f246-56f5-84b3-a7f222449b7d',true,'Соль, перец','Соль, перец',NULL,'1:по вкусу;'),
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'100:г;'),
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'100:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','32a10b45-e313-5e70-8494-75e4f950046c',true,'Любые на ваш выбор','Любые на ваш выбор','изюм, вишня, клюква, инжир, курага, чернослив','150:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','f43099f3-ac3a-53ba-8084-8bca3e764dd0',true,'Сок и цедра','Сок и цедра',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','fa35a350-38aa-51ec-9185-46bcc2c07bb8',true,'Сок гранатовый','Сок гранатовый',NULL,'150:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','6d7c7c6d-484e-5382-93a4-fb11a6297228',true,'Марципан','Марципан','Раздел: Ингредиенты для кекса','1:шт.;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','83918035-d6e2-5d58-9f5f-e75b7aee1fa9',true,'Мука пшенная','Пшенная мука','Раздел: Ингредиенты для кекса','50:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','d6a1eeb1-b75b-5365-ac54-dbac72700a31',true,'Мука зеленой гречки','Мука зеленой гречки','Раздел: Ингредиенты для кекса','50:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука','Раздел: Ингредиенты для кекса','50:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','0f4e75e9-6d57-5882-bdb7-13668f79ea15',true,'Крахмал тапиоки','Крахмал тапиоки','Раздел: Ингредиенты для кекса','30:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель','Раздел: Ингредиенты для кекса','1:ч.л.;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода','Раздел: Ингредиенты для кекса','0.5:ч.л.;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь','Раздел: Ингредиенты для кекса','150:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','c4c520bd-d2db-53a9-874b-989ce03ec69f',true,'Сухофрукты','Сухофрукты','Раздел: Ингредиенты для кекса','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','5c3e9db6-3f89-535f-9035-8e2b04ec6ce0',true,'Пряности','Пряности','Раздел: Ингредиенты для кекса','1:ч.л.;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','ac2b5831-f80b-56ce-bb00-014f1a40bf76',true,'Цедра апельсина и лимона','Цедра апельсина и лимона','по желанию, для ещё большей ароматности; Раздел: Ингредиенты для кекса','1:по вкусу;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко','Раздел: Ингредиенты для кекса','100:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','a7b21dea-293a-5652-96b4-72716b5ee432',true,'Сок из-под сухофруктов','Сок из-под сухофруктов','Раздел: Ингредиенты для кекса','50:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре','Раздел: Ингредиенты для кекса','70:г;'),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397','a58bff9f-b034-53c9-9a5b-2932e7f61d26',true,'Сахар','Сахар','Раздел: Ингредиенты для кекса','40:г;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','3ee68412-100f-5fae-a89a-a416fa5f1beb',true,'Цветная капуста','Цветная капуста','Раздел: Ингредиенты для риса из цветной капусты','1:по вкусу;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок','Раздел: Ингредиенты для риса из цветной капусты','1:шт.;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','ff2ccb17-41b5-5d43-b248-96ff694e30a0',true,'Имбирь','Имбирь','Раздел: Ингредиенты для риса из цветной капусты','1:ч.л.;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','Раздел: Ингредиенты для риса из цветной капусты','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('f0330825-3177-5e8e-9956-796217d9dc47','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко',NULL,'600:г;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','20bf6981-7489-5bad-950d-061c1bb5c974',true,'Куркума','Куркума','Раздел: Ингредиенты для риса из цветной капусты','0.5:ч.л.;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода','Раздел: Ингредиенты для риса из цветной капусты','1:по вкусу;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук','Раздел: Ингредиенты для карри','1:шт.;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','c9702512-8b24-5ee1-ba35-edd850041a3c',true,'Овощи по выбору','Овощи по выбору','у меня тыква и цукини; Раздел: Ингредиенты для карри','1:по вкусу;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','4920840b-dc84-5fa9-a148-9d74e8ff234a',true,'Ананас','Ананас','Раздел: Ингредиенты для карри','100:г;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','85759161-d1a1-5eec-af9b-1d341026533d',true,'Нут','Нут','Раздел: Ингредиенты для карри','150:г;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','f8715b4e-305d-5318-953f-ac8cc71d77c1',true,'Соль по вкусу','Соль по вкусу','Раздел: Ингредиенты для карри','1:по вкусу;'),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','Раздел: Ингредиенты для карри','1:стакан;'),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1','8b92da5b-892a-5c28-ae06-182d254687b2',true,'Мед','Мед',NULL,'100:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1','3dcb7477-4d6c-5511-a5bd-0e87d40c7258',true,'Кокосовые сливки','Кокосовые сливки',NULL,'80:г;'),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1','576705f0-eaa9-566f-b59c-85b72ca84ba7',true,'Миндальные лепестки','Миндальные лепестки',NULL,'100:г;'),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1','64a97ac3-4cdf-5198-ba0e-e33ff161050a',true,'Цукаты апельсиновые','Цукаты апельсиновые',NULL,'30:г;'),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1','9f32f189-d775-5836-a228-8cc178ad6b61',true,'Клюква вяленая','Клюква вяленая',NULL,'30:г;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','0a463f3f-f8fa-5dd3-9f28-d39b08d2373e',true,'Шпинат','Шпинат',NULL,'400:г;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','e9ff9e8a-992c-50d5-8adc-e3a8c177c473',true,'Зелёная стручковая фасоль','Зелёная стручковая фасоль',NULL,'200:г;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','be0e5010-6f3d-5b34-8686-3b95354f32cf',true,'Брокколи','Брокколи',NULL,'200:г;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','69e5987e-6cbd-5583-b42d-d2f86710fe22',true,'Специи','Специи',NULL,'1:по вкусу;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'2:шт.;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь',NULL,'1:шт.;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','3b57745e-ccdb-5972-ac50-7f5247e08655',true,'Перец сладкий запеченный','Перец сладкий запеченный',NULL,'1:шт.;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'1:по вкусу;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1.5:стакан;'),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'4:ст.л.;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'1:стакан;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'1:стакан;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель',NULL,'1:ч.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'0.5:ч.л.;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','bf5422ac-1f24-5072-a884-a95f187f4125',true,'Кокосовый урбеч','Урбеч кокосовый',NULL,'5:ст.л.;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'0.3333:стакан;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','1979006a-d7d7-505f-a029-4dea97dff18c',true,'Апельсиновый сок','Апельсиновый сок',NULL,'0.3333:стакан;'),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9','c4c520bd-d2db-53a9-874b-989ce03ec69f',true,'Сухофрукты','Сухофрукты',NULL,'1:по вкусу;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','a436c550-c6e6-5049-9319-3ede60d718cb',true,'Овсяные хлопья без глютена','Овсяные хлопья без глютена',NULL,'1.5:стакан;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'1:ст.л.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:по вкусу;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','5474fb48-c744-54be-907e-eab24e35113a',true,'Карри','Карри',NULL,'0.5:ч.л.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','ff2ccb17-41b5-5d43-b248-96ff694e30a0',true,'Имбирь','Имбирь',NULL,'0.25:ч.л.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','cddc2c2b-88b8-5c54-8e35-d4168a0af694',true,'Тмин','Тмин',NULL,'0.25:ч.л.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','67816b8d-b42a-5d1d-b5be-144282df4927',true,'Перец чили свежий','Перец чили свежий','по желанию','1:щепотка;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',true,'Помидор','Помидор',NULL,'2:шт.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',true,'Перец сладкий','Перец сладкий',NULL,'0.5:шт.;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','be0e5010-6f3d-5b34-8686-3b95354f32cf',true,'Брокколи','Брокколи',NULL,'1:по вкусу;'),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860','0ac426d9-0a4f-569c-89f3-e36e89d6c036',true,'Зелёный горошек','Зелёный горошек',NULL,'1:по вкусу;'),
	 ('37fef56b-1416-5636-b418-332ded70282e','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'80:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('37fef56b-1416-5636-b418-332ded70282e','83918035-d6e2-5d58-9f5f-e75b7aee1fa9',true,'Мука пшенная','Пшенная мука',NULL,'50:г;'),
	 ('37fef56b-1416-5636-b418-332ded70282e','ee3cce66-8244-5982-ad5d-a8b5eaedac23',true,'Семена льна','Семена льна',NULL,'20:г;'),
	 ('37fef56b-1416-5636-b418-332ded70282e','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'30:г;'),
	 ('37fef56b-1416-5636-b418-332ded70282e','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'80:г;'),
	 ('37fef56b-1416-5636-b418-332ded70282e','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре','лучше сделать самостоятельно из печёных яблок, см рецепт','1:по вкусу;'),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'1:кг;'),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы',NULL,'250:г;'),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03','69e5987e-6cbd-5583-b42d-d2f86710fe22',true,'Специи','Чеснок и специи',NULL,'1:по вкусу;'),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03','f8715b4e-305d-5318-953f-ac8cc71d77c1',true,'Соль по вкусу','Соль по вкусу',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','0020c500-2165-5b16-aba8-775ab26990bd',true,'Баклажан','Баклажан','Раздел: Ингредиенты для «бекона»','1:шт.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','d4495cb8-729c-51ed-bfbd-985a867bb2a9',true,'Шампиньоны','Шампиньоны','Раздел: Ингредиенты для «бекона»','200:г;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','d7946e27-c902-5271-8e8b-78a80714b651',true,'Черный перец','Черный перец',NULL,'1:по вкусу;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Ингредиенты для «бекона»','1:ст.л.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Ингредиенты для «бекона»','1:ст.л.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','28f6f887-a6bc-5516-8224-9d3e1ab0d1fd',true,'Кокосовые аминокислоты','Кокосовые аминокислоты','Раздел: Ингредиенты для «бекона»','2:ст.л.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','389cb4b0-c501-5f31-a2ca-9c0c5d1d2f8d',true,'Паприка копченая','Паприка копченая','Раздел: Ингредиенты для «бекона»','0.5:ч.л.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Чеснок сушеный','Раздел: Ингредиенты для «бекона»','0.5:ч.л.;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','2fc83e21-d021-5646-acf2-74e7f1745fad',true,'Острый перец','Острый перец','Раздел: Ингредиенты для «бекона»','1:по вкусу;'),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97','8eb1095b-6eac-5c60-baa7-53e95bf88beb',true,'Сырный соус из картошки','Сырный соус из картошки',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('1ab9222a-9d4d-5f6c-97a0-4152a2499ee3','55d9459f-854a-5c23-9b9a-c7e483c2ad3c',true,'Консервированная белая фасоль','Консервированная белая фасоль','Раздел: 1 вариант','1:по вкусу;'),
	 ('1ab9222a-9d4d-5f6c-97a0-4152a2499ee3','85759161-d1a1-5eec-af9b-1d341026533d',true,'Нут','Нут','Раздел: 2 вариант','500:г;'),
	 ('1ab9222a-9d4d-5f6c-97a0-4152a2499ee3','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода','Раздел: 2 вариант','1:по вкусу;'),
	 ('1ab9222a-9d4d-5f6c-97a0-4152a2499ee3','2b49d103-8220-5a4b-8c67-7c4d5d07bf80',true,'Аквафаба','Аквафаба','дегидрированная; Раздел: 3 вариант','1:по вкусу;'),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','с кокосовым будет более сливочный вкус + сыр будет плавиться в духовке','300:г;'),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404','71b5608c-304b-58f9-b5a8-51d8c4bfcb02',true,'Агар-агар','Агар-агар',NULL,'7:г;'),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404','0f4e75e9-6d57-5882-bdb7-13668f79ea15',true,'Крахмал тапиоки','Крахмал тапиоки',NULL,'12:г;'),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'10:г;'),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','bf8d085d-86d2-5160-b280-3f07544f5528',true,'Мак','Мак',NULL,'120:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'60:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','da004df1-ec9f-58cd-a1b6-7c4f272fce7a',true,'Молоко растительное','Молоко растительное',NULL,'60:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','5810a4fc-11b8-52ab-b0ea-964b8cdd922f',true,'Крахмал','Крахмал',NULL,'1:ст.л.;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'100:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','d6a1eeb1-b75b-5365-ac54-dbac72700a31',true,'Мука зеленой гречки','Мука зеленой гречки',NULL,'50:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'30:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','36aee9fe-196d-5b77-9b60-1a100466e652',true,'Псиллиум цельный','Псиллиум цельный',NULL,'10:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель',NULL,'1:ч.л.;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'100:г;'),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0','c608611c-22bd-5e37-a603-afa0f6525a5f',true,'Яблочное пюре','Яблочное пюре',NULL,'50:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'4:шт.;'),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Мука нутовая',NULL,'3:ст.л.;'),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'3:ст.л.;'),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'100:г;'),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5','5810a4fc-11b8-52ab-b0ea-964b8cdd922f',true,'Крахмал','Крахмал',NULL,'30:г;'),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'180:г;'),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'0.5:ч.л.;'),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5','69e5987e-6cbd-5583-b42d-d2f86710fe22',true,'Специи','Специи',NULL,'1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы','шампиньоны, вешенки','1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',true,'Перец сладкий','Перец сладкий',NULL,'1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','bf06a3d3-930d-53ef-9b07-25e3258628b1',true,'Лук красный','Лук красный',NULL,'1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:шт.;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','352d8afb-0f97-5de2-a460-968e36ecf244',true,'Оливки','Оливки',NULL,'1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',true,'Помидор','Помидор',NULL,'1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','6ecb4c22-d683-525f-9bd5-1a4564818887',true,'Томатная паста','Томатная паста','Раздел: Ингредиенты для соуса','3:ст.л.;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода','Раздел: Ингредиенты для соуса','2:ст.л.;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','Раздел: Ингредиенты для соуса','1:по вкусу;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Ингредиенты для соуса','1:ч.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','22e69402-41ae-5cd0-87ec-e68447cc11e5',true,'Орегано','Орегано','Раздел: Ингредиенты для соуса','1:ч.л.;'),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693','a4cf9371-34fc-5ed9-89eb-89919f1a7ae2',true,'Картофельное тесто','Картофельное тесто','Раздел: Сборка пиццы','1:шт.;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'100:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'50:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'40:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'50:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','bdb1f62b-9795-5f76-93ed-d821c53847af',true,'Кокосовое масло','Кокосовое масло',NULL,'30:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'60:г;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель',NULL,'1:ч.л.;'),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4','97beb515-943d-5a8b-9100-2f7a2471ba87',true,'Орехи','Орехи',NULL,'50:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','6a3a0636-a3c4-5eed-95a5-2593d2dffeca',true,'Рис','Рис','Раздел: Ингредиенты для риса','200:г;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','28f6f887-a6bc-5516-8224-9d3e1ab0d1fd',true,'Кокосовые аминокислоты','Кокосовые аминокислоты','аминосоус; Раздел: Ингредиенты для риса','1:ст.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','5474fb48-c744-54be-907e-eab24e35113a',true,'Карри','Карри','Раздел: Ингредиенты для риса','0.3333:ч.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Ингредиенты для риса','1:ч.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','be0e5010-6f3d-5b34-8686-3b95354f32cf',true,'Брокколи','Брокколи','Раздел: Ингредиенты для салата','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','e9ff9e8a-992c-50d5-8adc-e3a8c177c473',true,'Зелёная стручковая фасоль','Зелёная стручковая фасоль','Раздел: Ингредиенты для салата','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','2c8db663-98b8-59ae-83e9-376408cb5347',true,'Спаржа','Спаржа','Раздел: Ингредиенты для салата','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','c2201b6d-8bef-535a-8f0e-1487fdef6966',true,'Брюссельская капуста','Брюссельская капуста','Раздел: Ингредиенты для салата','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','477a150b-c7d7-5845-84e6-6ef853dca327',true,'Огурцы','Огурцы','Раздел: Ингредиенты для салата','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','1022bab1-90d2-57be-add0-cd9aa9d0a423',true,'Салат фриллис','Салат фриллис','Раздел: Ингредиенты для салата','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','460c3eb1-42a4-51f8-9f19-adc6c7c31d50',true,'Кунжут','Кунжут','Раздел: Ингредиенты для заправки','2:ст.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Ингредиенты для заправки','1:ст.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','c8403fed-a2c7-5fe7-b6bf-7abc0318a2df',true,'Лук зелёный','Лук зелёный','Раздел: Ингредиенты для заправки','1:по вкусу;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','ff2ccb17-41b5-5d43-b248-96ff694e30a0',true,'Имбирь','Имбирь','Раздел: Ингредиенты для заправки','1:ч.л.;'),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль','Раздел: Ингредиенты для заправки','1:по вкусу;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','c1e42c12-b4a4-5baf-a322-18dba72b4eeb',true,'Батат запеченный','Батат запеченный',NULL,'300:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое',NULL,'300:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','bf5422ac-1f24-5072-a884-a95f187f4125',true,'Кокосовый урбеч','Кокосовый урбеч',NULL,'50:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'60:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','936a779e-cd4b-5155-a500-60406cbd0c94',true,'Кэроб','Кэроб',NULL,'40:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'40:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','485c8656-bc51-510e-b851-5c4b7d976a14',true,'Мука зеленых бананов','Мука зеленых бананов',NULL,'80:г;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'1:ч.л.;'),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','c0b08006-0854-5fc8-9e94-4c07e59f35d0',true,'Морковь','Морковь',NULL,'1:шт.;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','48c8e9bf-f165-5a36-b310-222557ee72fb',true,'Капуста белокочанная','Капуста белокочанная',NULL,'1:по вкусу;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','1c73179c-f246-56f5-84b3-a7f222449b7d',true,'Соль, перец','Соль, перец',NULL,'1:по вкусу;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','c2144e2f-7e15-5b03-bb98-5a2c88e1ad9a',true,'Паприка','Паприка',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','23c3e842-e0e2-5e15-887f-c38f188a2fe7',true,'Чечевица красная','Чечевица красная',NULL,'1:стакан;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','6a3a0636-a3c4-5eed-95a5-2593d2dffeca',true,'Рис','Рис',NULL,'1:стакан;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','6ecb4c22-d683-525f-9bd5-1a4564818887',true,'Томатная паста','Томатная паста',NULL,'100:г;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'400:г;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','5273c607-82de-578c-a21e-83153982f22e',true,'Травы','Травы','орегано, итальянская смесь','1:по вкусу;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Чеснок сушеный',NULL,'1:ч.л.;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'75:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'75:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','ee3cce66-8244-5982-ad5d-a8b5eaedac23',true,'Семена льна','Семена льна',NULL,'50:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко',NULL,'120:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'35:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:щепотка;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель',NULL,'1:ч.л.;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'25:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'120:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','14bd91a5-5046-5625-b8d6-9092ba5dda87',true,'Масло растительное','Масло растительное',NULL,'20:г;'),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964','5273c607-82de-578c-a21e-83153982f22e',true,'Травы','Травы','орегано, розмарин, итальянская смесь','1:ч.л.;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы',NULL,'400:г;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','cebf6875-7da4-5c8f-8e55-f0b8b990cf21',true,'Тимьян','Тимьян',NULL,'1:ст.л.;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','7645fe13-3398-5057-a216-e91cf06ef461',true,'Овощной бульон','Овощной бульон',NULL,'500:г;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','8460acd2-6b82-56df-96d3-29a8605a182b',true,'Овсяные сливки','Овсяные сливки',NULL,'300:г;'),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','d56b7753-5bb3-5465-9b6b-d5a9a964688b',true,'Свекла','Свекла',NULL,'1:шт.;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','bf06a3d3-930d-53ef-9b07-25e3258628b1',true,'Лук красный','Лук красный',NULL,'1:по вкусу;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','c372b164-b120-5058-aec0-b9bd6cb0e974',true,'Зелень','Зелень','укроп, кинза','1:пучок;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'2:шт.;1:по вкусу;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','15e4f6d6-7076-55da-be9f-716e7fdc9dcd',true,'Грецкие орехи','Грецкие орехи',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','74dba30b-7495-5133-b3b6-efc8865da891',true,'Хмели-сунели','Хмели-сунели',NULL,'0.5:ч.л.;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','e339dae4-1a9d-58e4-99a8-ddc9111c5498',true,'Кориандр молотый','Кориандр молотый',NULL,'1:щепотка;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:щепотка;'),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','080247c9-68d3-5101-bb73-c46ae33f9172',true,'Кокосовая стружка','Кокосовая стружка','Раздел: Ингредиенты на 6 кексов','80:г;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука','Раздел: Ингредиенты на 6 кексов','80:г;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука','Раздел: Ингредиенты на 6 кексов','30:г;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода','Раздел: Ингредиенты на 6 кексов','0.5:ч.л.;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель','Раздел: Ингредиенты на 6 кексов','0.5:ч.л.;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','19a55aa8-5c1e-51fd-94f0-f69d724dad2e',true,'Банан','Банан','Раздел: Ингредиенты на 6 кексов','200:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','Раздел: Ингредиенты на 6 кексов','100:г;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','4c95ad06-5dc8-5aca-95f1-4e5ec6ca6abe',true,'Клубника','Клубника','Раздел: Ингредиенты на 6 кексов','120:г;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал','Раздел: Ингредиенты на 6 кексов','1:ст.л.;'),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Ингредиенты на 6 кексов','1:ст.л.;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','db6ef57e-caee-5092-8010-e04ef42a904c',true,'Рукола','Рукола',NULL,'50:г;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','62c68ca9-8b5c-56aa-8b73-0afe7d8ceac6',true,'Миндаль','Миндаль',NULL,'50:г;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:шт.;1:по вкусу;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','bf06a3d3-930d-53ef-9b07-25e3258628b1',true,'Лук красный','Лук красный',NULL,'1:по вкусу;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','352d8afb-0f97-5de2-a460-968e36ecf244',true,'Оливки','Оливки',NULL,'50:г;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','0de53086-87c0-5c69-8bbf-76e02e762c75',true,'Маринад от оливок','Маринад от оливок',NULL,'40:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','5474fb48-c744-54be-907e-eab24e35113a',true,'Карри','Карри',NULL,'0.5:ч.л.;'),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы','шампиньоны, вешенки','1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',true,'Перец сладкий','Перец сладкий',NULL,'1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:шт.;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','352d8afb-0f97-5de2-a460-968e36ecf244',true,'Оливки','Оливки',NULL,'1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',true,'Помидор','Помидор',NULL,'1:по вкусу;'),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','69e5987e-6cbd-5583-b42d-d2f86710fe22',true,'Специи','Специи','острый перец или чёрный перец','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'200:г;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'160:г;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'80:г;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'80:г;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','1999e680-7ac5-5285-b615-94b4ad501e77',true,'Псиллиум шелуха','Псиллиум шелуха',NULL,'4:ст.л.;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'2:ч.л.;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'2:ч.л.;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'2:ст.л.;'),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'450:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9630af28-8ade-5432-9788-b8d490c5de5f','9722b993-1d2a-5a85-b88a-7662e86bc7b9',true,'Хлопья','Хлопья',NULL,'100:г;'),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'500:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','3ef92a25-a13a-5092-ab2e-ca888882145c',true,'Кабачок','Кабачок','кабачок','200:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'100:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'100:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Мука рисовая',NULL,'100:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'150:г;'),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d','c8403fed-a2c7-5fe7-b6bf-7abc0318a2df',true,'Лук зелёный','Укроп, зеленый лук',NULL,'1:пучок;'),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'800:г;'),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'40:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('84b69143-798c-59da-8a8a-406525a6e5de','d59627c3-a64c-5c10-acc4-3e702de6706a',true,'Тыква','Тыква',NULL,'1:шт.;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','d59627c3-a64c-5c10-acc4-3e702de6706a',true,'Тыква','Тыква',NULL,'1:по вкусу;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','e1647ace-cb1e-5c7c-8ed7-a6f47f95b2ad',true,'Перец сладкий','Перец сладкий',NULL,'1:шт.;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','6b4ba6e3-74e2-54a9-8879-edf13451b244',true,'Помидоры черри','Помидоры черри',NULL,'250:г;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'1:шт.;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок',NULL,'1:по вкусу;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','c2af850e-9593-5627-acc6-92eed80dd7eb',true,'Яблоко','Яблоко',NULL,'0.25:шт.;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'1:стакан;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','56195042-f16f-5822-a651-62d43d3fee37',true,'Травы итальянские','Травы итальянские','готовая смесь или отдельно базилик, розмарин, орегано, тимьян','1:ст.л.;'),
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:ч.л.;1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('a6c563da-9bba-53a1-aafd-df53974c0cdd','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:по вкусу;'),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd','62c68ca9-8b5c-56aa-8b73-0afe7d8ceac6',true,'Миндаль','Миндаль',NULL,'100:г;'),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'500:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','b571b6f7-732a-5e48-bc2a-0698b9a88bfb',true,'Миндальное молоко','Миндальное молоко','Раздел: Заварная часть крема','300:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Заварная часть крема','50:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал','Раздел: Заварная часть крема','24:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','Раздел: Заварная часть крема','1:по вкусу;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','2b49d103-8220-5a4b-8c67-7c4d5d07bf80',true,'Аквафаба','Аквафаба','Раздел: Савоярди (печенье-бисквит)','160:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Савоярди (печенье-бисквит)','7:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука','Раздел: Савоярди (печенье-бисквит)','120:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука','Раздел: Савоярди (печенье-бисквит)','70:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель','без глютена; Раздел: Савоярди (печенье-бисквит)','2.5:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','a8f06049-4e76-5faf-b56f-8390febd646a',true,'Сметана кокосовая','Сметана кокосовая','Раздел: Сметанный крем','450:г;'),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6','f44e3551-3273-5986-a3fc-8f8ef023f827',true,'Заварная часть','Заварная часть','Раздел: Сметанный крем','1:по вкусу;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','1c3ae44c-6348-57ed-9ea6-3b0773b3e0ed',true,'Мешочек для молока','Мешочек для молока','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.','1:по вкусу;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','f2e9c448-c9cf-5f14-9a95-91235d826ab3',true,'Пергамент для выпечки','Пергамент для выпечки','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.','1:по вкусу;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','e833d9f3-06a2-5989-a583-e325837945b5',true,'Форма для сборки торта','Форма для сборки торта','Раздел: Тот же бисквит савоярди, но пропитан ягодным пюре. Крем сделан более нейтральным по вкусу и более светлым - для лучшего сочетания с ягодами. В прослойку добавлен клубничный джем. Можно собрать в виде торта "Фрезье" - с цельными ягодами клубники по периметру и на разрезе - и он без сомнений вызовет восторг у семьи и гостей.','1:по вкусу;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','Раздел: Заварная часть крема','400:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура','Раздел: Заварная часть крема','100:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','6748e8ff-7d0f-5397-82ab-476688288098',true,'Картофельный крахмал','Картофельный крахмал','Раздел: Заварная часть крема','35:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','Раздел: Заварная часть крема','1:по вкусу;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','4c95ad06-5dc8-5aca-95f1-4e5ec6ca6abe',true,'Клубника','Клубника','Раздел: Клубничная начинка','200:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый','Раздел: Клубничная начинка','40:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','2b49d103-8220-5a4b-8c67-7c4d5d07bf80',true,'Аквафаба','Аквафаба','Раздел: Савоярди (печенье-бисквит)','160:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок','Раздел: Савоярди (печенье-бисквит)','7:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука','Раздел: Савоярди (печенье-бисквит)','130:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука','Раздел: Савоярди (печенье-бисквит)','80:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель','без глютена; Раздел: Савоярди (печенье-бисквит)','2.5:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','a8f06049-4e76-5faf-b56f-8390febd646a',true,'Сметана кокосовая','Сметана кокосовая','Раздел: Сметанный крем','300:г;'),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f','f44e3551-3273-5986-a3fc-8f8ef023f827',true,'Заварная часть','Заварная часть','Раздел: Сметанный крем','1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','a8f06049-4e76-5faf-b56f-8390febd646a',true,'Сметана кокосовая','Сметана кокосовая',NULL,'250:г;'),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','b238e952-6b98-59ed-b16e-607a509ada2e',true,'Вареная сгущенка','Вареная сгущенка',NULL,'50:г;'),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','212eb083-c0ee-56fa-a173-762130cdb096',true,'Блины "шоколадные"','Блины "шоколадные"',NULL,'5:шт.;'),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','487a8345-e9e6-582c-8bd3-f4b0ad244cd6',true,'Сметанный крем','Сметанный крем',NULL,'1:по вкусу;'),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','19a55aa8-5c1e-51fd-94f0-f69d724dad2e',true,'Банан','Банан',NULL,'1:шт.;'),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e','ed426749-cf2d-555d-ac6a-eea3365deae5',true,'Орехи грецкие','Орехи грецкие','или любые по вкусу','50:г;'),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','17-19% жирности','300:г;'),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'100:г;'),
	 ('f0330825-3177-5e8e-9956-796217d9dc47','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'100:г;'),
	 ('f0330825-3177-5e8e-9956-796217d9dc47','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'100:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('f0330825-3177-5e8e-9956-796217d9dc47','0f4e75e9-6d57-5882-bdb7-13668f79ea15',true,'Крахмал тапиоки','Крахмал тапиоки',NULL,'30:г;'),
	 ('f0330825-3177-5e8e-9956-796217d9dc47','36aee9fe-196d-5b77-9b60-1a100466e652',true,'Псиллиум цельный','Псиллиум цельный',NULL,'5:г;'),
	 ('f0330825-3177-5e8e-9956-796217d9dc47','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'20:г;1:по вкусу;'),
	 ('f0330825-3177-5e8e-9956-796217d9dc47','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:щепотка;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'35:г;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'75:г;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','0f4e75e9-6d57-5882-bdb7-13668f79ea15',true,'Крахмал тапиоки','Крахмал тапиоки',NULL,'17:г;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','936a779e-cd4b-5155-a500-60406cbd0c94',true,'Кэроб','Кэроб',NULL,'15:г;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко',NULL,'400:г;'),
	 ('7e931153-f384-589b-92be-b6cb7455a603','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'40:г;1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','13900dce-3e69-555f-b088-0de3c5191a04',true,'Киноа','Киноа белая',NULL,'100:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','190930b0-2b41-554f-b454-0e92b1a8e1fb',true,'Мука бурого риса','Мука бурого риса',NULL,'100:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'40:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'400:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'3:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'10:г;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'15:г;1:по вкусу;'),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'50:г;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'150:г;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'75:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'30:г;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','ee3cce66-8244-5982-ad5d-a8b5eaedac23',true,'Семена льна','Семена льна',NULL,'90:г;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко',NULL,'600:г;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'20:г;1:по вкусу;'),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:щепотка;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','54b71c87-9feb-5500-8376-71646a51c747',true,'Блины','Блины',NULL,'8:шт.;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','55e34641-ed9b-5ae9-9872-7eb2d758c9f2',true,'Творожный сыр','Творожный сыр',NULL,'300:г;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','0a463f3f-f8fa-5dd3-9f28-d39b08d2373e',true,'Шпинат','Шпинат',NULL,'300:г;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','17-19%','150:г;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'150:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','7c3f5db2-fc29-5e9a-a248-c20ea9fbd9fc',true,'Помидор','Помидор','очищенные/протёртые','200:г;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','1c73179c-f246-56f5-84b3-a7f222449b7d',true,'Соль, перец','Соль, перец',NULL,'1:по вкусу;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','1ae1e4d0-4d21-5a16-b62f-2c0aab59e785',true,'Сушеные травы','Сушеные травы','орегано или смесь итальянских трав','1:ч.л.;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Чеснок сушеный',NULL,'0.5:ч.л.;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','9874594b-71a3-50cc-b314-09e52b719724',true,'Мускатный орех','Мускатный орех',NULL,'0.25:ч.л.;'),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'1:по вкусу;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'1:стакан;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'2:ст.л.;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'2:ст.л.;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','2c3c04f0-535b-564a-9561-deba13ded4ae',true,'Молоко ореховое','Молоко ореховое',NULL,'2:стакан;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','c2af850e-9593-5627-acc6-92eed80dd7eb',true,'Яблоко','Яблоко',NULL,'1:шт.;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','87ebb42f-67b1-5e5e-9ae5-151955c6d64b',true,'Сироп','Сироп',NULL,'1:ст.л.;1:по вкусу;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'1:ч.л.;'),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('fcb65d25-0886-5071-a188-33d37d560668','54b71c87-9feb-5500-8376-71646a51c747',true,'Блины','Блины',NULL,'6:шт.;'),
	 ('fcb65d25-0886-5071-a188-33d37d560668','facb1701-378a-5464-8d47-0125500e0f3a',true,'Начинкажюльен грибной','Начинкажюльен грибной',NULL,'1:по вкусу;'),
	 ('fcb65d25-0886-5071-a188-33d37d560668','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'1:по вкусу;'),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6','62c68ca9-8b5c-56aa-8b73-0afe7d8ceac6',true,'Миндаль','Миндаль',NULL,'200:г;'),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6','991a021e-f869-533e-9aca-6ce7cb2a56f6',true,'Кешью','Кешью',NULL,'100:г;'),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'600:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:по вкусу;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','0a02c657-918d-571d-bcc7-638b4a1dd439',true,'Для блинов: блины "жюльен"','Для блинов: блины "жюльен"','Раздел: Где ещё использовать','1:по вкусу;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','2235fb17-4534-5d00-8535-0c1ca798e9d9',true,'Грибы','Грибы','шампиньоны/вешенки','300:г;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук',NULL,'50:г;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'150:г;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко',NULL,'150:г;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','4ed1a2d5-6ff6-5273-a230-0905784ab869',true,'Чеснок сушеный','Соль, чеснок сушеный',NULL,'1:по вкусу;'),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b','c9775cc5-7f53-5501-bea9-9909fd23259a',true,'Твердый сыр','Твердый сыр',NULL,'1:по вкусу;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','23773894-cdfb-5005-b5a6-0c76a5bf02f6',true,'Овсяные хлопья','Овсяные хлопья',NULL,'1:стакан;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','3a1cd39e-cec7-543b-9759-bde7daec371d',true,'Пшено','Пшено',NULL,'1:по вкусу;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('da9c1cec-6648-54ac-9977-5658237b1608','a111617d-d2b4-51d9-adf5-8d74d7a85a41',true,'Тыквенное пюре','Тыквенное пюре',NULL,'1:по вкусу;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'1:ч.л.;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'3:ст.л.;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1:по вкусу;'),
	 ('da9c1cec-6648-54ac-9977-5658237b1608','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'1:ст.л.;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'140:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','1979006a-d7d7-505f-a029-4dea97dff18c',true,'Апельсиновый сок','Апельсиновый сок',NULL,'170:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','742b1e2c-5773-5aaa-b50e-4ab543be0b01',true,'Миндальная мука','Миндальная мука',NULL,'60:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','da004df1-ec9f-58cd-a1b6-7c4f272fce7a',true,'Молоко растительное','Молоко растительное',NULL,'170:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','83918035-d6e2-5d58-9f5f-e75b7aee1fa9',true,'Мука пшенная','Пшенная мука',NULL,'30:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','57aeb289-29b4-53a8-a2fa-7d829d726779',true,'Масло виноградной косточки','Масло виноградной косточки',NULL,'30:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'60:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','a9b0e13a-3138-5a17-a37b-e09bed645d1e',true,'Сахар панела','Сахар панела',NULL,'90:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','b887a3d4-5bd4-5746-9ecc-92553a79663b',true,'Псиллиум','Псиллиум','цельный','18:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'25:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','cb3b977b-e4c9-53fa-9383-1b716de9d08c',true,'Разрыхлитель','Разрыхлитель',NULL,'6:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','c4c520bd-d2db-53a9-874b-989ce03ec69f',true,'Сухофрукты','Сухофрукты',NULL,'150:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','312cb550-e8a8-5271-89c3-06d8ed9359cb',true,'Сода','Сода',NULL,'8:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','e0e8a13c-abfd-55b0-9199-41c301eb5f6d',true,'Цедра апельсина','Цедра апельсина',NULL,'1:шт.;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','9fbb86ee-092d-5fa9-b4dc-e845e3019b89',true,'Сахар тростниковый','Сахар тростниковый',NULL,'100:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','2b49d103-8220-5a4b-8c67-7c4d5d07bf80',true,'Аквафаба','Аквафаба',NULL,'20:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','a58bff9f-b034-53c9-9a5b-2932e7f61d26',true,'Сахар','Сахар',NULL,'100:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'50:г;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','e91a3f6f-3727-5704-84f2-52632fe4e5a6',true,'Ксантановая камедь','Ксантановая камедь',NULL,'0.2:ч.л.;'),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7','6d7c7c6d-484e-5382-93a4-fb11a6297228',true,'Марципан','Марципан',NULL,'1:по вкусу;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','991a021e-f869-533e-9aca-6ce7cb2a56f6',true,'Кешью','Кешью',NULL,'220:г;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'70:г;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','3a1cd39e-cec7-543b-9759-bde7daec371d',true,'Пшено','Пшено',NULL,'300:г;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','bf5422ac-1f24-5072-a884-a95f187f4125',true,'Кокосовый урбеч','Кокосовый урбеч',NULL,'100:г;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'80:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'50:г;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','a30ae4ef-07f9-502f-ad14-c1ed5ca5042b',true,'Цедра лимона','Цедра лимона',NULL,'1:шт.;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','экстракт/паста/семена','1:по вкусу;'),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2','e2c11382-0412-5636-95f9-86c53b83629e',true,'Начинка','Начинка','сухофрукты, орехи','1:по вкусу;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','62c68ca9-8b5c-56aa-8b73-0afe7d8ceac6',true,'Миндаль','Миндаль',NULL,'300:г;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','991a021e-f869-533e-9aca-6ce7cb2a56f6',true,'Кешью','Кешью',NULL,'150:г;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'1000:г;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','6e0729ea-b639-5cbf-81bb-62077b288e9a',true,'Лимонный сок','Лимонный сок',NULL,'90:г;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','66516ef7-5f43-5dd5-810f-7599a6f5dfaf',true,'Сахар кокосовый','Сахар кокосовый',NULL,'60:г;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','a30ae4ef-07f9-502f-ad14-c1ed5ca5042b',true,'Цедра лимона','Цедра лимона',NULL,'1:шт.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','52a9f464-487d-5694-950f-ad8055950445',true,'Ваниль','Ваниль','экстракт/паста/семена','1:ч.л.;'),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70','e2c11382-0412-5636-95f9-86c53b83629e',true,'Начинка','Начинка','сухофрукты, орехи','1:по вкусу;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'100:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'50:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','d6a1eeb1-b75b-5365-ac54-dbac72700a31',true,'Мука зеленой гречки','Мука зеленой гречки',NULL,'25:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','b887a3d4-5bd4-5746-9ecc-92553a79663b',true,'Псиллиум','Псиллиум','мука','8:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'4:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','12b65300-9582-587b-80fa-30d4e720f95e',true,'Вода','Вода',NULL,'160:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','c28bad38-775f-51c8-9d09-877e3b039384',true,'Вешенки','Вешенки','или шампиньоны; Раздел: Ингредиенты для начинки с картошкой и грибами','120:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель','Раздел: Ингредиенты для начинки с картошкой и грибами','320:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','Лук','Раздел: Ингредиенты для начинки с картошкой и грибами','40:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','4e5188eb-ac95-502a-81cf-8a4a7973d1d6',true,'Перец черный','Перец черный','Раздел: Ингредиенты для начинки с картошкой и грибами','0.25:ч.л.;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','a128b1f3-0b92-50a8-8b66-3b69bc7cc6ca',true,'Фасоль красная готовая','Фасоль красная готовая','Раздел: Ингредиенты для начинки с фаршем из фасоли','240:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','a7536601-aa13-59ab-ac5b-0f2f04e9050f',true,'Чеснок','Чеснок','Раздел: Ингредиенты для начинки с фаршем из фасоли','6:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','6ecb4c22-d683-525f-9bd5-1a4564818887',true,'Томатная паста','Томатная паста','Раздел: Ингредиенты для начинки с фаршем из фасоли','45:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','22f9818d-0447-5fb7-b7c9-0c363bc291cd',true,'Капуста квашеная','Капуста квашеная','Раздел: Ингредиенты для начинки с фаршем из фасоли','100:г;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','74dba30b-7495-5133-b3b6-efc8865da891',true,'Хмели-сунели','Хмели-сунели','Раздел: Ингредиенты для начинки с фаршем из фасоли','1:ч.л.;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','93e3f557-a326-5b31-9bcf-2b6a1c9ca04a',true,'Перец черный молотый','Перец черный молотый','Раздел: Ингредиенты для начинки с фаршем из фасоли','0.5:ч.л.;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','389cb4b0-c501-5f31-a2ca-9c0c5d1d2f8d',true,'Паприка копченая','Паприка копченая','Раздел: Ингредиенты для начинки с фаршем из фасоли','1:ч.л.;'),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06','928ef100-3feb-5e17-9351-d5a42f58aaed',true,'Пажитник молотый','Пажитник молотый','по желанию; Раздел: Ингредиенты для начинки с фаршем из фасоли','0.5:ч.л.;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'80:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'80:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','d854a993-11cf-5ffd-bbca-d1c059ac666f',true,'Мука нутовая','Нутовая мука',NULL,'30:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','a6c35db6-8b66-50c2-8e79-f6aec67143fa',true,'Тапиоковый крахмал','Тапиоковый крахмал',NULL,'20:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','901d9bb5-ee88-5b3b-88c7-8cef61ae9cc7',true,'Кокосовое молоко','Кокосовое молоко','нежирное','400:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','6b1ad9a9-aca8-5772-ba17-e3cda280ef4f',true,'Картофель','Картофель',NULL,'200:г;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','c372b164-b120-5058-aec0-b9bd6cb0e974',true,'Зелень','Зелень','укроп и зелёный лук','1:пучок;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','9a346879-8bfd-5ec0-9ece-3fd7d055b41a',true,'Лук','По желанию грибы и лук',NULL,'1:по вкусу;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'15:г;');
INSERT INTO public."RecipeIngredients" ("RecipeId","IngredientId","IsRequired","Name","Description","Comment","Quantities") VALUES
	 ('1755d74c-998a-545b-b725-0439d99037fb','d6a1eeb1-b75b-5365-ac54-dbac72700a31',true,'Мука зеленой гречки','Мука зеленой гречки',NULL,'80:г;'),
	 ('1755d74c-998a-545b-b725-0439d99037fb','fe445b5d-0f48-52a7-9949-6751f7b5286b',true,'Мука овсяная','Мука овсяная',NULL,'70:г;'),
	 ('1755d74c-998a-545b-b725-0439d99037fb','1d5bb7b9-4276-5b02-97b0-9ef7c52e860e',true,'Мука рисовая','Рисовая мука',NULL,'50:г;'),
	 ('1755d74c-998a-545b-b725-0439d99037fb','002952e4-01f8-5883-acb7-9a1e3e7c7fa0',true,'Молоко','Молоко',NULL,'600:г;'),
	 ('1755d74c-998a-545b-b725-0439d99037fb','914e0df9-971c-51e3-a314-5031c16cea03',true,'Соль','Соль',NULL,'1:по вкусу;'),
	 ('1755d74c-998a-545b-b725-0439d99037fb','65c5a320-8738-50ac-b17a-5c203b6ac1c1',true,'Сироп топинамбура','Сироп топинамбура',NULL,'1:по вкусу;');
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63',1,NULL,'Все сухие ингредиенты смешать в миске. Добавить жидкий подсластитель (+ ореховую пасту, пюре, если добавляете). Перемешать, чтобы жидкие ингредиенты равномерно покрыли все сыпучие.',NULL),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63',2,NULL,'Количество подсластителя и пасты можно регулировать по вкусу, главное, чтобы они покрывали все хлопья.',NULL),
	 ('95c477b5-b4c3-5f98-8219-25c043c64a63',3,NULL,'Выложить ровным слоем на пергамент и запечь при 180гр до румяности 15-20 минут. Во время запекания можно перемешать 1-2 раза, чтобы гранола запекалась равномернее.',NULL),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e',1,NULL,'Хорошо разогреть сковороду с толстым дном. Всыпать немного амаранта и распределить тонким слоем',NULL),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e',2,NULL,'Сразу же плотно накрыть сковороду крышкой. Жарить, периодически встряхивая сковороду, пока зёрна не перестанут лопаться',NULL),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e',3,NULL,'Готовую порцию амаранта сразу пересыпать в миску. Повторить с остальными зёрнами',NULL),
	 ('a0c83758-63a4-584c-bcb3-8f12803a1b2e',4,NULL,'Когда все зёрна будут приготовлены, дать им полностью остыть, затем пересыпать в герметичную ёмкость / пакет на замке, чтобы попкорн сохранил хрусткость',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',1,NULL,'Фасоль и пшено отварить до готовности, но не разваривайте сильно, чтобы они были более плотные и давали текстуру «фаршу»',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',2,NULL,'Картофель отварить на пару и сразу размять в пюре',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',3,NULL,'Лук, чеснок и грибы обжарить на сухой сковороде до выделения влаги, потушить до испарения влаги. Добавить соль, специи, соус барбекю и по желанию аминосоус. Если добавляете соус, соли может понадобиться меньше.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',4,NULL,'Фасоль, пшено и грибы измельчить блендером или в комбайне, оставляя немного неоднородных кусочков. Вмешать картофельное пюре',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',5,NULL,'Кинзу/петрушку порубить и добавить к массе. Попробовать на вкус, при необходимости добавить больше соли/специй',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',6,NULL,'Сформировать котлеты на пергаменте, отправить в духовку при 180гр примерно на 20 минут.',NULL),
	 ('6286bded-34d4-5888-aa91-ece6bf635a9b',7,NULL,'Смазать соусом барбекю и запечь ещё минут 15-20, ориентируясь на вашу духовку (чтобы котлеты запеклись и не были мягкими внутри) Как разморозить: Для запечённых котлет - разморозить в духовке или на сковороде. Для сырых - запечь в духовке, добавив 10-15 ми',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',1,NULL,'Лук и чеснок измельчить, обжарить на сухой сковороде пару минут, затем убавить огонь и добавить немного воды',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',2,NULL,'Добавить нарезанные помидоры и томатную пасту, готовить до размягчения помидоров',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',3,NULL,'Добавить соль, специи',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',4,NULL,'Чечевицу промыть и всыпать к томатам',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',5,NULL,'Влить воду, перемешать, накрыть крышкой и готовить на среднем огне до готовности чечевицы. При необходимости долить больше воды',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',6,NULL,'При желании можно добавить обжаренные грибы или другие овощи',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',7,NULL,'Кабачки нарезать вдоль тонкими слайсами, выложить на дно формы, сверху начинку из чечевицы и слегка смазать сыром',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',8,NULL,'Повторить слои необходимое количество раз, верхний слой кабачков покрыть сыром',NULL),
	 ('d3e41d2f-5953-5224-8073-3913086b8ed5',9,NULL,'Поставить в разогретую до 180 градусов духовку. Запекать около 40 минут, чтобы сверху образовалась румяная корочка',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',1,NULL,'Воду довести до кипения',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',2,NULL,'Всыпать в кипящую воду муку, слегка разровнять, добавить соль и оставить на минуту на медленном огне',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',3,NULL,'Тесто накрыть плёнкой/полотенцем и полностью остудить',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',4,NULL,'Небольшой шарик раскатать между двумя листами пергамента/пищевой плёнки как можно тоньше',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',5,NULL,'Вырезать ножом по диаметру тарелки ровные лепёшки',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',6,NULL,'Жарить на сухой сковороде на высоком огне с двух сторон до румяности',NULL),
	 ('2b46ce3f-b336-5e12-9601-24be9c40efd3',7,NULL,'Готовые лепёшки складывать друг на друга и заворачивать в полотенце, если хотите, чтобы они были более мягкими, а не сухими и ломкими',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',1,NULL,'Для тыквенного пюре тыкву запечь до мягкости, очистить от кожуры и пробить блендером',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',2,NULL,'Семена льна измельчить до состояния муки, залить водой, перемешать и оставить, пока смешиваете остальные ингредиенты',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',3,NULL,'Всю муку, крахмал, соду и соль смешать венчиком',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',4,NULL,'Пюре соединить с семенами льна, объединить с мучной смесью',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',5,NULL,'Если тесто получается слишком густое/сухое, можно добавить немного воды. Из более густого теста получится более плотный хлеб',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',6,NULL,'В конце влить лимонный сок и перемешать, сразу выложить в форму и поставить в разогретую до 180гр духовку. Если у вас нет формы, можно сделать хлеб или булочки без формы, тогда тесто должно быть более густое',NULL),
	 ('aa19f9cb-75fe-5089-943e-d1609e5605c0',7,NULL,'Выпекать около 30-40 минут. Дать полностью остыть Такой хлеб получается очень вкусным, если его поджарить на сковороде или в тостере, с хрустящей корочкой и мягким мякишем внутри',NULL),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528',1,NULL,'Замороженный горошек выложить на разогретую сковороду и готовить до испарения влаги и размягчения горошка. Не готовьте слишком долго, чтобы он не потерял цвет. Также можно слегка отварить',NULL),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528',2,NULL,'Готовый горошек положить в блендер со всеми остальными ингредиентами и взбить до максимально однородной консистенции. Регулируйте густоту, добавляя воду по чуть-чуть',NULL),
	 ('1fe31b2b-0cfe-5af5-8fa9-7804f62c8528',3,NULL,'Попробуйте на вкус, можно добавить больше остроты/кислинки/соли/зелени/специй, а также по желанию кунжутную пасту или свежий кунжут',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('98638408-7fec-598e-90a3-eba036fa54df',1,NULL,'Овощи очистить, нарезать и отварить на пару до мягкости',NULL),
	 ('98638408-7fec-598e-90a3-eba036fa54df',2,NULL,'Приготовленные и слегка остывшие овощи сложить в блендер с остальными ингредиентами. Взбить до однородной консистенции, по желанию добавить больше соли/специй/лимона',NULL),
	 ('a6a19570-7d3d-5f04-b1a4-7964199d2b55',1,NULL,'Баклажан запечь до мягкости (кусочками или целиком)',NULL),
	 ('a6a19570-7d3d-5f04-b1a4-7964199d2b55',2,NULL,'Сливы немного размочить в горячей воде, в зависимости от того, насколько они жёсткие',NULL),
	 ('a6a19570-7d3d-5f04-b1a4-7964199d2b55',3,NULL,'Все ингредиенты взбить в блендере-измельчителе, добавляя воду до желаемой густоты и однородной консистенции',NULL),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b',1,NULL,'Яблоки нарезать дольками или небольшими кубиками и сбрызнуть лимонным соком',NULL),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b',2,NULL,'Сухие ингредиенты смешать отдельно венчиком, при необходимости просеять',NULL),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b',3,NULL,'Добавить пюре и сироп, замесить тесто, в конце влить лимонный сок и перемешать',NULL),
	 ('3e1fe235-0b1c-5dc1-8826-c48d2f5bbb7b',4,NULL,'Добавить яблоки, перемешать, чтобы все кусочки покрылись тестом Если делаете в сковороде: В разогретую сковороду (у меня 26 см) положить пергамент и выложить сверху тесто. Накрыть крышкой. Уменьшить огонь на низкий. Держать 30-40 минут, тесто должно немног',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',1,NULL,'Муку, крахмал, сахар и соль смешать венчиком в миске',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',2,NULL,'Добавить масло, перетереть в крошку, стараясь не нагревать руками',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',3,NULL,'Добавить яблочное пюре, перемешать, собрать тесто и проверить текстуру',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',4,NULL,'Добавить воду по чуть-чуть до нужной текстуры',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',5,NULL,'Завернуть в плёнку, убрать в холодильник примерно на 15-30 минут',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',6,NULL,'После холодильника слегка размять тесто, чтобы оно стало более податливое. Раскатать желаемой толщиной',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',7,NULL,'Вырезать кольцом дно и отдельно сформировать бортики. Если делаете в форме с дном, то просто выложите в неё пласт теста, придайте форму',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',8,NULL,'Уберите в холодильник на время приготовления начинки',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',9,NULL,'Яблоки нарезать мелким кубиком, добавить лимонный сок и корицу (+ по желанию другие пряности)',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',10,NULL,'Кокосовый сахар и молоко нагреть до кипения',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',11,NULL,'Всыпать яблоки, уменьшить огонь до среднего и томить яблоки в карамели до полумягкости',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',12,NULL,'Воду смешать с крахмалом и влить к яблокам, помешивая. Уварить до загущения пару минут (главное, чтобы яблоки не разварились в пюре)',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',13,NULL,'Оставить начинку до сборки (можно убрать в холодильник)',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',14,NULL,'Кешью предварительно замочить в горячей воде на полчаса. Либо можно залить холодной водой на ночь. После замачивания промыть',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',15,NULL,'Все ингредиенты соединить в блендере, взбить до однородной текстуры',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',16,NULL,'Вылить на основу из теста, поставить в разогретую до 170гр духовку. Выпекать 40-50 минут. Не передержите, чтобы не пережарить тесто, но начинка должна хорошо пропечься. В середине она может оставаться немного сырой',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',17,NULL,'Полностью остудить чизкейк при комнатной температуре. Выложить сверху яблочную начинку',NULL),
	 ('7283e450-c0f6-5c95-b0c1-95ef3595a040',18,NULL,'Поставить в холодильник до застывания чизкейка',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',1,NULL,'Манго очистить, поместить в блендер вместе с молоком, агаром и сиропом. Взбить до состояния однородного пюре',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',2,NULL,'Переложить в сотейник, нагреть на среднем огне до кипения',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',3,NULL,'Добавить стружку и кокосовый урбеч, вымешать, проварить ещё 1-2 минуты',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',4,NULL,'Переложить массу сразу в контейнер, застеленный пищевой плёнкой/пергаментом или в специальные силиконовые формы для батончиков',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',5,NULL,'Убрать в холодильник на 2-4 часа или на ночь до полного застывания. После застывания нарезать на батончики и убрать в холод до глазировки',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',6,NULL,'Манго измельчить в пюре вместе с молоком, сиропом и агаром',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',7,NULL,'Массу нагреть до кипения на среднем огне, проварить 10 секунд',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',8,NULL,'Перед глазировкой батончики можно подморозить 10-15 минут, чтобы они крепче держались и не разваливались',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',9,NULL,'В горячую глазурь окунуть батончик со всех сторон, стряхнуть лишнюю глазурь, выложить батончики на пергамент, чтобы потом легко снять',NULL),
	 ('294d90f5-1e1b-55aa-ad6f-03370016b216',10,NULL,'Поставить в холодильник до полного застывания.',NULL),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',1,NULL,'Манго пробить блендером вместе с сиропом и агаром до состояния пюре. Можно использовать размороженные кусочки манго',NULL),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',2,NULL,'Переложить пюре в сотейник и нагреть на среднем огне, постоянно помешивая, до кипения',NULL),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',3,NULL,'Горячее пюре соединить с кокосовой пастой и взбить блендером до однородной консистенции',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',4,NULL,'Переложить в миску/контейнер и поставить в холодильник на пару часов, можно на ночь',NULL),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',5,NULL,'Из охлаждённой массы слепить конфеты, вкладывая в серединку орешек',NULL),
	 ('dad21299-0790-556e-ad01-9527b5ebcfe8',6,NULL,'Готовую конфету сразу обвалять в стружке. Снова охладить в холодильнике',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',1,NULL,'Чеснок, лук и имбирь измельчить/нарезать и обжарить на сухой сковороде 1-2 минуты.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',2,NULL,'Добавить морковь, нарезанную соломкой, и грибы, обжарить до выделения влаги из грибов.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',3,NULL,'Добавить соль и специи.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',4,NULL,'Пекинскую капусту нашинковать, добавить к овощам, притушить несколько секунд и выключить огонь. Овощи должны быть состояния альденте.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',5,NULL,'Готовую фасоль немного размять и вмешать к овощам',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',6,NULL,'Зелёный лук нарезать и вмешать к овощам',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',7,NULL,'Лист бумаги быстро окунуть в тёплую воду, долго не держать, иначе бумага будет рваться.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',8,NULL,'Выложить на доску или другую поверхность, положить на него нори, сверху начинку.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',9,NULL,'Завернуть сначала края, затем положить второй лист нори и завернуть ролл.',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',10,NULL,'Выложить роллы на противень, застеленный пергаментом, смазать сверху с помощью кисточки смесью сиропа и воды (2:1) или небольшим количеством масла (это нужно, чтобы роллы получились более хрустящие). Если нет кисточки, можно слегка окунуть поверхность ролл',NULL),
	 ('fbe85f17-fe01-5c90-93a0-cb0542b12215',11,NULL,'Присыпать кунжутом и поставить запекаться при 180гр на 20-30 минут, чтобы сверху образовалась хрустящая корочка. Кроме того, вы можете делать эти роллы в сыром, незапечённом виде',NULL),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7',1,NULL,'Всё смешать в сотейнике с помощью венчика и уварить на среднем огне до кипения. Соус должен слегка загустеть. Густоту можно регулировать количеством крахмала.',NULL),
	 ('1b364ee5-2e09-53d4-97cc-2572dee3c6c7',2,NULL,'Полностью остудить и хранить в холодильнике',NULL),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',1,NULL,'Духовку разогреть до 160 градусов.',NULL),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',2,NULL,'Все ингредиенты соединить в блендере и взбить до однородности. Можно попробовать и отрегулировать сладость/кислинку.',NULL),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',3,NULL,'Выложить в форму для запекания. У меня стеклянная, ничем не смазываю и не покрываю.',NULL),
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',4,NULL,'Поставить в духовку на 45-50 минут.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('b36d0bd6-bb78-5c6d-8b3e-951c825edd3a',5,NULL,'Достать и полностью остудить. Затем можно резать или поставить в холодильник. Мне нравится охлаждённой',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',1,NULL,'Лук нарезать, чеснок измельчить, морковь натереть на тёрке, перец нарезать соломкой',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',2,NULL,'Лук и чеснок обжарить на сухой сковороде пару минут. Добавить морковь и перец, специи и соль, перемешать. Обжарить ещё 3-5 минут, пока овощи не начнут прилипать',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',3,NULL,'Сыр натереть на тёрке',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',3,NULL,'Баклажан проткнуть вилкой по всей поверхности. Затем прокатить, надавливая руками (можно также отбить широким ножом)',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',4,NULL,'Сделать надрез вдоль, не доходя до противоположной стороны, раскрыть и выложить плотно начинку',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',5,NULL,'Уложить баклажаны в сковороду',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',6,NULL,'Всё смешать, при необходимости добавить воду, если у вас густые томаты.',NULL),
	 ('0c7f6843-1afb-5503-88c0-84a5f0168736',7,NULL,'Заполнить промежутки между баклажанами, встряхнуть сковороду, накрыть крышкой и томить на медленном огне 1,5 часа',NULL),
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c',1,NULL,'Муку немного обжарить на сковороде, чтобы раскрылся аромат.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c',2,NULL,'Влить сироп, смешать до пластичной массы, продолжая нагревать на среднем огне.',NULL),
	 ('a3b872bd-333a-589a-a2d5-e87890ef1e7c',3,NULL,'Когда масса объединится, снять с огня и завернуть в плёнку/пергамент, чтобы марципан остыл, но не засыхал.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',1,NULL,'Нарезать подходящими кусочками, залить соком, чтобы полностью покрывал сухофрукты, добавить цедру. В сотейнике нагреть до кипения, томить 1 минуту, выключить огонь и оставить минут на 15. Затем слить сок.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',2,NULL,'Духовку разогреть до 170-180гр',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',3,NULL,'Всю муку, разрыхлитель, соду и пряности смешать венчиком.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',4,NULL,'Молоко, сок, пюре и сахар смешать отдельно и влить к сухой смеси.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',5,NULL,'Замесить тесто (смотрите на текстуру на видео, если тесто получится гуще, добавьте ещё немного сока или молока).',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',6,NULL,'Добавить морковь и сухофрукты, всё объединить, выложить в форму.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',7,NULL,'Сделать углубление вдоль всей формы, выложить в него марципан. Прикрыть сверху тестом и разровнять поверхность.',NULL),
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',8,NULL,'Поставить в духовку (время выпекания зависит от вашей духовки и высоты формы. У меня заняло 40 минут)',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('9e71f2e6-b05a-5197-9ae2-6843beaad397',9,NULL,'Полностью остудить',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',1,NULL,'Цветную капусту нарезать на соцветия, поместить в чашу измельчителя, пробить до крупной крошки',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',2,NULL,'Готовить на сухой сковороде вместе с измельчённым чесноком, имбирём и куркумой/карри',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',3,NULL,'Когда начнёт прилипать, добавить немного воды, посолить и готовить до желаемой текстуры (до мягкости или же оставить слегка хрустящей)',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',4,NULL,'Лук обжарить с измельчённым чесноком и имбирём на сухой сковороде',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',5,NULL,'Добавить овощи, нарезанные мелким кубиком и все специи, немного воды и готовить минут 7-10 до полуготовности овощей',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',6,NULL,'Добавить нут, нарезанный кубиком ананас, посолить, влить молоко и томить ещё 7-10 минут, чтобы молоко уварилось и загустело. При необходимости добавить больше жидкости',NULL),
	 ('b54f78af-f2dc-5471-97e7-76d0d264b2f9',7,NULL,'Сервировать вместе с капустой / другим гарниром',NULL),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1',1,NULL,'Мёд и сливки соединить в сотейнике, довести до кипения и уваривать пару минут, чтобы масса начала густеть.',NULL),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1',2,NULL,'Всыпать все наполнения и продолжать перемешивать, пока лепестки не покроются равномерно карамелью.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1',3,NULL,'Выложить массу ровным слоем на пергамент и выпекать при 170-180гр около 15-20 минут. Пласт должен стать равномерно золотистым.',NULL),
	 ('10a3bcd6-0938-5dc8-9425-c46fa550b9a1',4,NULL,'Остудить и вырезать формочками печенье или поломать руками. Если печенье получилось слишком мягкое и тягучее, можно убрать в холодильник, чтобы оно стало более твёрдое',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',1,NULL,'Если вы берёте замороженные овощи, разморозьте их на сковороде. Шпинат разморозьте отдельно и слейте лишнюю жидкость.',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',2,NULL,'Овощи протушить до полуготовности вместе с чесноком и специями. Добавить соль',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',3,NULL,'Добавить шпинат и протушить на огне ещё минуту до испарения лишней влаги',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',4,NULL,'Переложить в форму для запекания и свободно распределить',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',5,NULL,'Все готовые овощи, соль, молоко и воду взбить в блендере до однородной консистенции',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',6,NULL,'Всыпать муку и перемешать. Количество муки можно корректировать в зависимости от густоты заливки',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',7,NULL,'Вылить в форму сверху начинки и распределить',NULL),
	 ('d23d826a-a5ed-5a94-b2b0-5253f88c0fc6',8,NULL,'Поставить в разогретую до 170гр духовку, запекать 35-45 минут, чтобы вся заливка схватилась и перестала быть жидкой',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',4,NULL,'Взять раскатанное картофельное тесто',NULL),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9',1,NULL,'Смешать все сухие ингредиенты, добавить кокосовый урбеч и перетереть в крошку вместе с мукой',NULL),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9',2,NULL,'Влить сок апельсина и замесить однородное тесто. Тесто должно быть пластичное и не липнуть к рукам. По желанию вмешать сухофрукты',NULL),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9',3,NULL,'Сформировать печенье желаемой формы',NULL),
	 ('3921d654-1f92-57c2-8e8f-766e92b6ade9',4,NULL,'Выпекать при 180гр 10-15 минут, достать, смазать смесью кокосовых сливок и сахара (удобнее всего кисточкой). Допечь ещё около 15 минут до румяности',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',1,NULL,'Все сухие ингредиенты смешать венчиком, добавить растопленное масло, перетереть в крошку.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',2,NULL,'Влить воду и перемешать до мягкой, крошащейся текстуры теста.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',3,NULL,'Распределить крошку по противню, запекать 10-15 минут до равномерно румяного состояния. Горячая крошка будет оставаться мягкой, после остывания она станет жёсткой и хрустящей.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',4,NULL,'Часть печенья измельчить в блендере или растолочь до состояния однородной мелкой крошки. Оставить пару горсток крупной крошки для хрустящей текстуры, и чтобы она создавала вкрапления на разрезе',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',5,NULL,'Сливки с кэробом нагреть в сотейнике, помешивая венчиком, чтобы разошлись комочки.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',6,NULL,'Добавить урбеч, смешать венчиком до однородной текстуры.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',7,NULL,'Смешать получившуюся массу с крошкой. Добавить крупные кусочки печенья, по желанию можно добавить также нарубленные орехи и сухофрукты.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',8,NULL,'Должна получиться пластичная масса, которую можно формировать.',NULL),
	 ('d6dcfc96-bb42-5703-977a-4bf651cdd5c8',9,NULL,'Выложить брусочком, завернуть в плёнку/пергамент в форме колбаски, убрать в холодильник на несколько часов до застывания',NULL),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',1,NULL,'Овсяные хлопья и муку поместить в форму для запекания. Залить кипятком так, чтобы все хлопья смочились водой. Перемешать, оставить набухать',NULL),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',2,NULL,'Лук мелко нарезать, чеснок измельчить. Обжарить на сухой сковороде вместо со специями. Когда начнёт прилипать, добавить немного воды и тушить до испарения воды',NULL),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',3,NULL,'Овощи нарезать кусочками, выложить в сковороду, добавить соль. Готовить 3-5 минут. При необходимости добавлять немного воды. Здесь нужно лишь слегка протушить, чтобы вкусы объединились',NULL),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',4,NULL,'Переложить овощи к хлопьям, перемешать, разровнять. Можно украсить сверху нарезанным помидором',NULL),
	 ('f1498fe8-d34d-596c-a811-7632f1f3c860',5,NULL,'Поставить в разогретую до 180гр духовку, запекать 30-35 минут до румяной хрустящей корочки и мягкости овощей',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',1,NULL,'Все ингредиенты для теста кроме молока смешать венчиком',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('37fef56b-1416-5636-b418-332ded70282e',2,NULL,'Влить молоко, замесить тесто. Тесто должно собираться в шар, быть пластичным и не крошиться, чтобы его можно было сразу раскатывать. Если тесто получилось липким/жидким - добавить овсяную муку',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',3,NULL,'Раскатать тесто на пергаменте (при необходимости присыпать рисовой мукой) толщиной около 5 мм в форме круга',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',4,NULL,'Разрезать круг на равные сектора (у меня получилось 12 частей).',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',5,NULL,'Смазать каждую часть яблочным пюре, присыпать щепоткой кокосового сахара и дроблёными орехами. Аккуратно свернуть в рулетик, начиная с широкой части',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',6,NULL,'Выложить на противень хвостиком вниз, чтобы рулетики не раскрылись.',NULL),
	 ('37fef56b-1416-5636-b418-332ded70282e',7,NULL,'Выпекать в разогретой до 180гр духовке 20-25 минут',NULL),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',1,NULL,'Картофель отварить на пару до мягкости, размять в однородное пюре без добавления воды',NULL),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',2,NULL,'Добавить соль и специи по вкусу (например, сушёный чеснок, перец, мускатный орех)',NULL),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',3,NULL,'При необходимости можно добавить в пюре пару ложек крахмала (тапиоковый или картофельный), если картошка не достаточно крахмалистая, и чтобы тесто лучше лепилось',NULL),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',4,NULL,'Лук и чеснок обжарить с грибами на сковороде пару минут. При необходимости добавить немного воды. Затем добавить соль, специи и тушить до испарения влаги из грибов',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',5,NULL,'Из пюре сделать лепёшку, выложить на неё начинку и руками сформировать шар. Придавить на противне в форме котлетки',NULL),
	 ('8377833b-e5b5-5457-bb24-0744fc494a03',6,NULL,'Запекать при 180-190 градусах 30-40 минут до румяной корочки. Немного остудить',NULL),
	 ('da9c1cec-6648-54ac-9977-5658237b1608',4,NULL,'В конце добавить сок лимона, перемешать',NULL),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97',1,NULL,'Баклажан разрезать пополам вдоль. Каждую половинку нарезать тонкими ломтиками (5-7 мм). Грибы тоже нарезать ломтиками',NULL),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97',2,NULL,'Остальные ингредиенты смешать в миске. Перемешать овощи с маринадом, выложить на противень и запекать при 180 гр до румяности около 30-40 минут',NULL),
	 ('051c2023-3e1a-5c06-bb32-ab21bd386f97',3,NULL,'Баклажаны нарезать кусочками',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',1,NULL,'Перемешать все ингредиенты (кроме зелени, если добавляете) в сотейнике венчиком, чтобы крахмал и агар равномерно распределились',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',2,NULL,'Поставить на огонь и постоянно помешивать, чтобы смесь не пригорала. Довести до кипения на среднем огне',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',3,NULL,'После появления пузырей и устойчивого кипения уварить ещё минимум 30 секунд. Для более плотной консистенции можно проварить дольше. И наоборот - для более мягкого сыра можно снять с огня сразу после закипания',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',4,NULL,'В конце приготовления вмешать мелко нарубленную зелень (по желанию)',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',5,NULL,'Вылить горячую массу в контейнер, миску или силиконовую форму и поставить в холодильник минимум на 2 часа или на ночь до уплотнения',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',6,NULL,'Кешью промыть и замочить в горячей воде на 30 минут - 1 час. Воду слить и ещё раз промыть',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',7,NULL,'Все ингредиенты поместить в мощный блендер и взбить до однородной консистенции',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',8,NULL,'Поставить на огонь и постоянно помешивать венчиком, чтобы смесь не пригорала. Довести до кипения на среднем огне',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',9,NULL,'После появления пузырей и устойчивого кипения уварить ещё минимум 30 секунд. Для более плотной консистенции можно проварить дольше. И наоборот - для более мягкого сыра можно снять с огня сразу после закипания',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',10,NULL,'В конце приготовления вмешать мелко нарубленную зелень (по желанию)',NULL),
	 ('61cd5fce-9807-5bdb-9c81-0185a7181404',11,NULL,'Вылить горячую массу в контейнер, миску или силиконовую форму и поставить в холодильник минимум на 3 часа или на ночь до уплотнения',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',1,NULL,'Мак залить горячей водой и оставить минимум на 30 минут набухнуть. Затем воду максимально слить через мелкое сито',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',2,NULL,'Слегка пробить замоченный мак погружным блендером, но не в пасту',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',3,NULL,'Молоко смешать с сиропом и крахмалом венчиком. Соединить вместе с маком в сотейнике, довести до кипения. Проварить на среднем огне около 1 минуты до загущения, постоянно помешивая',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',4,NULL,'Переложить в миску и полностью остудить (можно убрать в холод), чтобы начинка стала более плотная',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',5,NULL,'Все сухие ингредиенты смешать венчиком',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',6,NULL,'Отдельно смешать молоко, сироп и пюре, влить к сухим ингредиентам и замесить тесто. Оставить тесто на 10 минут для набухания',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',7,NULL,'Проверить текстуру теста. Оно должно получиться плотным, чтобы можно было раскатывать. Если тесто слишком мягкое, добавьте немного муки или псиллиума до нужной текстуры',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',8,NULL,'Раскатать тесто на пергаменте в форме прямоугольника толщиной 5-7 мм. При необходимости в процессе слегка присыпать рисовой мукой',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',9,NULL,'Распределить начинку равномерно по пласту теста, оставив с одного края 3-4 см без начинки',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',10,NULL,'Завернуть рулет, помогая пергаментом, начиная с края с начинкой. Аккуратно переложить на противень, смазать смесью молока и сиропа. Поставить в разогретую духовку при 180гр ~ на 30 минут. Тесто должно стать золотистым и твердым',NULL),
	 ('e337372d-a654-5725-a1f6-496c035b9ca0',11,NULL,'Полностью остудить и можно нарезать',NULL),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab',1,NULL,'Картофель отварить на пару, размять в однородное пюре без добавления воды',NULL),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab',2,NULL,'Добавить муку, крахмал и соль, замесить тесто',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab',3,NULL,'Раскатать лепёшку на пергаменте желаемой толщины. Можно как раскатать скалкой, так и распределить руками',NULL),
	 ('2f4197ee-49b1-5547-8474-3d923ecde6ab',4,NULL,'Запечь в разогретой до 180гр духовке до румяности',NULL),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5',1,NULL,'Все ингредиенты смешать тщательно венчиком, чтобы не осталось комочков.',NULL),
	 ('0345144f-264b-507c-8aa1-9e50e8e37db5',2,NULL,'Вылить часть теста на хорошо прогретую сковороду. Жарить на огне выше среднего с двух сторон.',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',1,NULL,'Грибы и перец обжарить вместе с чесноком, добавить немного воды, посолить и тушить до полумягкости.',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',2,NULL,'Помидоры, оливки и красный лук нарезать отдельно',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',5,NULL,'Смазать половину томатным соусом, выложить сверху тушёные овощи и нарезанные свежие овощи. Посыпать натёртым сыром.',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',6,NULL,'Аккуратно сложить лепёшку пополам и склеить края. Если тесто ломается, просто склеивайте трещины руками.',NULL),
	 ('5d59f6ca-fe30-53b4-931e-0b23bc55e693',7,NULL,'Поставить в разогретую до 200гр духовку примерно на 25-30 минут. Ориентируйтесь на цвет корочки, она должна стать равномерно румяной',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',1,NULL,'Все сухие ингредиенты (кроме молока, масла и орехов) смешать в миске венчиком',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',2,NULL,'Молоко и масло немного подогреть, чтобы масло растопилось',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',3,NULL,'Влить к сухой смеси и замесить тесто. Должно получиться пластичное мягкое тесто. Оставить на 5 минут, чтобы жидкость максимально впиталась, и проверить консистенцию. Тесто не должно растекаться или наоборот крошиться и быть слишком сухим',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',4,NULL,'Орехи немного порубить (предварительно можно промыть и подсушить в духовке), всыпать в тесто, перемешать',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',5,NULL,'Выложить тесто в форме колбаски на противень и немного приплюснуть',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',6,NULL,'Поставить в духовку примерно на 25 минут при 180°. Тесто подрумянится сверху, но внутри останется мягким.',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',7,NULL,'Остудить батончик 20-30 минут (для того, чтобы тесто не раскрошилось при нарезке)',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',8,NULL,'Нарезать аккуратно острым ножом пилящими движениями на слайсы толщиной около 1,5 см. Тесто может немного крошиться, это не страшно',NULL),
	 ('e8511bbb-1c1f-527c-8656-1287d01a7bc4',9,NULL,'Разложить сухарики по противню и запечь ещё примерно 10-15 минут, чтобы они стали твёрдыми и хрустящими. Полностью остудить',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',1,NULL,'Все ингредиенты смешать, чтобы соус равномерно покрыл каждую рисинку',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',2,NULL,'Распределить рис тонким слоем на пергамент или в форму для запекания',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',3,NULL,'Поставить в духовку при 180° с конвекцией на 25-30 минут. В процессе запекания перемешивать, чтобы рис запекался равномерно. Готовность определяется по золотистому цвету.',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',4,NULL,'Полностью остудить до хрустящего состояния',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',5,NULL,'Брокколи, фасоль и спаржу нарезать, бланшировать в воде на сковороде до состояния альденте (чтобы они немного размягчились, но оставались хрустящие). Можно использовать и в свежем виде',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',6,NULL,'Брюссельскую капусту тонко нашинковать в сыром виде.',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',7,NULL,'Огурец можно нарезать кусочками или в виде лапши',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',8,NULL,'Кунжут обжарить на сковороде до раскрытия аромата. Остудить и измельчить в кофемолке/блендере до состояния муки',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',9,NULL,'Смешать кунжут с остальными ингредиентами. При необходимости добавить немного воды до нужной консистенции',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',10,NULL,'Попробовать на вкус, отрегулировать соль, кислоту, сладость. Оставить на 10-15 минут, чтобы заправка настоялась',NULL),
	 ('92923225-3284-56cb-b8f4-84324b2eefdd',11,NULL,'Смешать все компоненты салата с заправкой, посыпать рисом',NULL),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',1,NULL,'Запечённый батат, молоко, растопленный урбеч и сахар поместить в блендер и взбить до однородной консистенции',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',2,NULL,'Перелить массу в миску, добавить кэроб, соду и миндальную муку, тщательно перемешать',NULL),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',3,NULL,'Муку бананов добавлять постепенно, пока тесто не приобретёт тягучую, не растекающуюся текстуру. От количества муки также будет зависеть плотность готового кекса.',NULL),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',4,NULL,'В самом конце влить сок лимона, перемешать и сразу выложить в форму. Для кекса можно брать форму поменьше',NULL),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',5,NULL,'Поставить в разогретую до 170° духовку. Выпекать примерно 30-40 минут (в зависимости от толщины), или пока кекс не будет упругим при надавливании',NULL),
	 ('17348153-5532-5d54-ab7a-d90954dd6e4f',6,NULL,'Полностью остудить при комнатной температуре, а затем убрать в холодильник, если вы хотите более плотный и тягучий брауни (урбеч/паста уплотнится в холодильнике)',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',1,NULL,'Лук нарезать, морковь натереть на тёрке или нарезать тонкой соломкой. Чеснок измельчить',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',2,NULL,'Обжарить лук, морковь и чеснок пару минут (на сковороде или в кастрюле). При необходимости добавить немного воды',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',3,NULL,'Капусту нашинковать мелко, добавить к овощам, немного притушить',NULL),
	 ('da9c1cec-6648-54ac-9977-5658237b1608',5,NULL,'Печь на сухой, хорошо разогретой сковороде',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',16,NULL,'Варите на высоком огне',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',4,NULL,'Всыпать промытую чечевицу, добавить соль и специи, перемешать. Влить немного воды, накрыть крышкой и тушить на среднем огне до полуготовности капусты и чечевицы (капуста должна оставаться немного хрустящей)',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',5,NULL,'Остудить, добавить готовый рис, перемешать. Сформировать шарики/котлетки, плотно прижимая, чтобы они не разваливались. Выложить в форму для запекания и поставить в духовку при 180° на 10 минут',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',6,NULL,'Томатную пасту слегка обжарить в сотейнике, помешивая венчиком, чтобы она раскрыла аромат и приобрела более мягкий вкус (не обязательно, можно просто смешать все ингредиенты)',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',7,NULL,'Добавить соль, специи. Влить молоко и на среднем огне довести до кипения, постоянно помешивая. Проверить вкус, при необходимости скорректировать',NULL),
	 ('ba3189e4-dc73-5969-bc9b-8f9898680976',8,NULL,'Достать голубцы из духовки, залить соусом и поставить в духовку ещё на 15-20 минут (до румяной корочки)',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',1,NULL,'Семена льна измельчить в кофемолке и залить молоком (или водой), оставить',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',2,NULL,'В это время смешать муку и соль венчиком',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',3,NULL,'Добавить сироп и льняное яйцо. Замесить тесто и проверить текстуру: тесто должно быть плотное и пластичное, не крошиться и не липнуть к рукам. При необходимости добавить немного воды или муки',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',4,NULL,'Поставить воду в кастрюле на огонь. Из кусочков теста скатать колбаски, сформировать колечки. Можно также оставить просто в форме палочек',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',5,NULL,'Кипящую воду подсолить и выложить в неё сушки. Слегка перемешать, чтобы сушки не склеились',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',6,NULL,'После закипания проварить ещё 30-60 секунд и достать сушки, выложить на полотенце/салфетки, чтобы убрать лишнюю воду',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',7,NULL,'Переложить на противень. По желанию посыпать/окунуть в мак, кунжут. Поставить в разогретую до 190-200° духовку и выпекать 30-40 минут',NULL),
	 ('ab7996ab-cd57-5ec7-aa9e-0f407fd28964',8,NULL,'В процессе можно перевернуть на другую сторону, чтобы внутри сушки лучше и быстрее просушились Их вы тоже можете делать как солёными, так и сладкими',NULL),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5',1,NULL,'Лук нарезать, чеснок измельчить - обжарить в кастрюле около 5 минут на огне выше среднего до более золотистого состояния. Если пригорает - добавляйте по чуть-чуть воды',NULL),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5',2,NULL,'Добавить нарезанные грибы, обжарить до выделения влаги и готовить, пока вода не выпарится (у меня ушло около 7 минут)',NULL),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5',3,NULL,'Добавить соль, тимьян, перемешать. Влить бульон, накрыть крышкой, довести до кипения и убавить огонь на средний. Варить 5-7 минут',NULL),
	 ('d18f2d13-f3ba-5b3d-945f-f81f1d1125e5',4,NULL,'Влить сливки, снова довести до кипения и проварить 3-5 минут, чтобы суп слегка загустел',NULL),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74',1,NULL,'Лук нарезать тонко. Зелень нарубить мелко, чеснок измельчить. Сложить в одну миску, добавить лимонный сок, специи и соль, перемешать.',NULL),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74',2,NULL,'Оставить на 15-30 минут пропитаться. При желании можно промять руками, так лук станет мягче и больше похож на маринованный.',NULL),
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74',3,NULL,'Готовую остывшую свёклу натереть на тёрке, заправить готовой смесью.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('26a3837e-d28d-5a9c-ae18-3d6e6d594b74',4,NULL,'Орехи мелко порубить, добавить к салату и всё тщательно перемешать. Дать постоять ещё минут 15 и подавать.',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',1,NULL,'Кокосовую стружку частично измельчить в кофемолке (40-50г) до состояния муки, но чтобы не превратилась в пасту',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',2,NULL,'В миске смешать венчиком рисовую, нутовую муку (просейте, если есть комочки), измельчённую и цельную стружку, соду и разрыхлитель',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',3,NULL,'Банан пробить блендером вместе с молоком до однородного пюре',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',4,NULL,'Влить к сухой смеси и замесить тесто, проверить текстуру. Должно получиться достаточное густое, не стекать с ложки, а падать. Если получается слишком густое, добавьте немного молока или воды',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',5,NULL,'Клубнику нарезать крупным кубиком и смешать с крахмалом, чтобы она не выделяла много жидкости в тесто',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',6,NULL,'Добавить клубнику в тесто вместе с лимонным соком и аккуратно перемешать. Выложить в формы для маффинов до края или одну металлическую/силиконовую и вставить в ячейки бумажные формы или пергамент',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',7,NULL,'В каждый кекс утопить сверху половинку клубники',NULL),
	 ('a8967aba-1684-5c36-b9f3-ad46c48d7bbb',8,NULL,'Поставить в духовку, разогретую до 180 градусов. Выпекать около 35-40 минут до румяной корочки. Кексы должны быть упругими при надавливании со всех сторон. Полностью остудить',NULL),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c',1,NULL,'Орехи промыть, замачивать не обязательно. Все ингредиенты сложить в измельчитель или блендер с s-образным лезвием',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c',2,NULL,'Взбить, чтобы все ингредиенты измельчились равномерно, оставив лёгкую крупинчатую текстуру',NULL),
	 ('cd6958bd-10ea-5846-94e2-814e8c1e156c',3,NULL,'Маринад или воду добавляйте по консистенции и потребностям блендера, но чтобы не было жидко',NULL),
	 ('4b04bcba-f0ec-5443-b03a-fafc09b4c737',1,NULL,'Яблоки разрезать пополам, вырезать сердцевину и выложить на противень, застеленный пергаментом, или в форму для запекания',NULL),
	 ('4b04bcba-f0ec-5443-b03a-fafc09b4c737',2,NULL,'Запечь до мягкости при t 180-200гр. После остывания взбить в блендере вместе с кожурой до однородной текстуры',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',1,NULL,'Лук, грибы и перец обжарить вместе с чесноком до испарения влаги из грибов. Добавить немного воды, посолить и тушить минут 5',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',2,NULL,'Добавить резаные помидоры, оливки, по желанию фасоль, специи и тушить ещё около 3-5 минут до готовности овощей и испарения лишней влаги',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',3,NULL,'Сыр натереть на тёрке',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',4,NULL,'Часть теста вылить на хорошо прогретую сковороду тонким слоем и сразу выложить на половину лепёшки немного сыра, затем овощи и снова сыр. Так начинка будет более сочная и тягучая',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',5,NULL,'Накрыть начинку второй половиной лепёшки, слегка прижать и перевернуть. Прожарить с другой стороны ещё немного',NULL),
	 ('f1b8739e-dd70-57a5-a449-f28590d4e2e3',6,NULL,'Горячая лепёшка получается хрустящей с сочной начинкой. Подавать, разрезав на две части, с зеленью и свежими овощами',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',1,NULL,'К моменту приготовления теста духовка должна быть разогрета до 180-190 градусов и готова начинка, если готовите пирожки или пиццу',NULL),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',2,NULL,'Все сухие ингредиенты смешать тщательно венчиком. Если есть комочки - просеять через сито',NULL),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',3,NULL,'Воду и лимонный сок смешать отдельно, добавить к сухим ингредиентам и тщательно перемешать. Оставить тесто на 10 минут, чтобы впиталась вся влага',NULL),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',4,NULL,'Проверить консистенцию теста. Оно должно быть довольно мягкое, липкое и пластичное, при этом держать форму. Если у вас получается плотное тесто, добавляйте постепенно чуть больше воды',NULL),
	 ('788d02a5-5548-5a8c-b6be-8bd17533f7a3',5,NULL,'Выпекать 25-35 минут в зависимости от типа, размера изделий и вашей духовки Булочки и пирожки можно замораживать в испечённом виде, затем размораживать сразу в духовке.',NULL),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f',1,NULL,'Хлопья промыть под холодной водой и отправить в блендер',NULL),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f',2,NULL,'Добавить холодную воду, чтобы молоко сильно не нагревалось при взбивании, и не приобрело слишком склизкую текстуру.',NULL),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f',3,NULL,'Взбить в течение ~ 1 минуты до максимальной однородности',NULL),
	 ('9630af28-8ade-5432-9788-b8d490c5de5f',4,NULL,'Процедить через сито. Можно отжать через мешочек, в зависимости от рецепта',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',1,NULL,'Цукини и сыр натереть на крупной тёрке, зелень мелко нарезать',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',2,NULL,'Смешать вместе с солью, чтобы всё распределилось равномерно',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',3,NULL,'Муку смешать с водой венчиком до однородной текучей консистенции. При необходимости добавьте больше воды или муки',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',4,NULL,'Аккуратно объединить тесто с кабачково-сырной смесью, попробовать на соль',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',5,NULL,'Выложить на хорошо разогретую сковороду, убавить огонь ~ на 6/10 и накрыть крышкой',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',6,NULL,'Готовить около 10 минут с одной стороны, проверить, что низ хорошо зарумянился, и перевернуть на другую сторону',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',7,NULL,'При желании присыпать сверху тёртым сыром, снова накрыть крышкой и готовить ещё около 10 минут (ориентируйтесь на вашу плиту, сковороду и т.д.)',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',8,NULL,'Проверить, что вторая сторона тоже прожарилась до румяности и переложить лепёшку на тарелку',NULL),
	 ('19170d35-69ba-5694-ad6a-ef4d4984969d',9,NULL,'Дать ей немного остыть, затем можно сложить пополам (или оставить так) и нарезать В таком формате лепёшка получается максимально похожей на хачапури или пирог с сыром. Но вы можете делать лепёшки любых размеров и толщины, например, в виде оладьев. Найдите ',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',1,NULL,'Кокосовое молоко (любой температуры от холодной до комнатной) налить в миску',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',2,NULL,'В молоко разом влить весь лимонный сок',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',3,NULL,'Аккуратно перемешать ложкой, молоко должно сразу начать сворачиваться. Перемешать так, чтобы сок равномерно распределился, но не очень активно, чтобы сохранялось расслоение',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',4,NULL,'Подготовить для отвешивания: мешочек для орехового молока',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',5,NULL,'Перелить молоко в мешочек и подвесить его над раковиной или миской. Если используете марлю и сито, то сито разместить на миску так, чтобы между ними было пространство для стекания жидкости',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',6,NULL,'Оставить на 2 часа - ночь, в зависимости от желаемой плотности сметаны. Примеры текстуры показаны в видео',NULL),
	 ('0554e70d-ff93-51ee-bffb-f8dd0704a34e',7,NULL,'Отвешенное молоко переложить в миску, перемешать до однородности и убрать в холодильник до полной стабилизации',NULL),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',1,NULL,'Тыкву помыть и нарезать с кожурой дольками/крупными кусками. Можно запечь также целиком',NULL),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',2,NULL,'Выложить в форму для запекания или на противень, застеленный пергаментом',NULL),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',3,NULL,'Поставить в разогретую до 180-200 градусов духовку примерно на 30-40 минут. Запечь до мягкости. Время зависит от размера кусочков и сорта тыквы. Периодически проверяйте',NULL),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',4,NULL,'Запечённую тыкву остудить и снять / срезать кожуру',NULL),
	 ('84b69143-798c-59da-8a8a-406525a6e5de',5,NULL,'Измельчить мякоть в чаше блендера или погружным блендером до однородной текстуры',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('84b69143-798c-59da-8a8a-406525a6e5de',6,NULL,'Переложить в баночку или контейнер. Если хотите заморозить - в формочки для заморозки Можно разложить по формочкам порционно и хранить в морозилке до 3 месяцев в герметичном пакете или контейнере.',NULL),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd',1,NULL,'Миндаль промыть под холодной водой и залить кипятком для набухания примерно на 1 час (можно на полчаса, если у вас мощный блендер, или дольше).',NULL),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd',2,NULL,'После замачивания слить воду, промыть, положить в блендер. Добавить 500 г холодной воды.',NULL),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd',3,NULL,'Взбить в течение ~ 1 минуты до максимальной однородности.',NULL),
	 ('1e3b65a6-81a1-5bcc-bf08-3c6fe42508fd',4,NULL,'Отжать через мешочек для орехового молока.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',1,NULL,'Все ингредиенты объединить в сотейнике при помощи венчика.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',2,NULL,'Поставить на огонь, нагреть на среднем огне до кипения, постоянно помешивая.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',3,NULL,'Снять с огня, перелить в миску, накрыть плёнкой/пакетом/пергаментом в контакт.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',4,NULL,'Остудить и поставить в холодильник минимум на 3 часа до полного охлаждения.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',5,NULL,'Разогреть духовку до 180 градусов, противень застелить пергаментом. Подготовить кондитерский мешок для формирования савоярди.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',6,NULL,'Муку, крахмал и разрыхлитель смешать венчиком в миске до однородности. Если есть комочки - просеять через сито.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',7,NULL,'Аквафабу поместить в высокую миску, начать взбивать.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',8,NULL,'Когда аквафаба приобретёт вид однородной мягкой "пивной" пены, влить сок лимона и продолжить взбивать на максимальной скорости.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',9,NULL,'Взбивать активно до состояния устойчивых пиков. На это может уйти до 5-10 минут. Аквафаба должна стать упругой и сопротивляться, когда вы водите по ней венчиком.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',10,NULL,'Начать вливать тонкой струйкой сироп, продолжая взбивать. Влить весь сироп в течение 1 минуты и ещё немного взбить. Меренга должна быть устойчивой и пластичной.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',11,NULL,'Всыпать смесь муки в меренгу в 3 захода, каждый раз аккуратно перемешивая лопаткой, движениями снизу вверх. Не мешайте слишком долго, чтобы меренга не опала. Если тесто получается слишком жидкое, добавьте немного рисовой муки. Тесто должно сохранять форму ',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',12,NULL,'Переложить готовое тесто в кондитерский мешок, отрезать кончик и отсадить печенье в форме савоярди. Поставить в духовку ~ на 15 минут. Готовое печенье упругое при надавливании, может быть румяное по краям, но в центре остаётся светлым. Остудить перед сборк',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',13,NULL,'Заварной крем размягчить лопаткой/ложкой, взбить немного миксером, чтобы разбить комочки. Блендером пробивать нельзя, иначе крем станет жидким.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',14,NULL,'Сметану переложить в миску, удобную для взбивания. Взбивать, постепенно увеличивая скорость, до насыщения воздухом и уплотнения. На сметане должен появиться объёмный рисунок.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',15,NULL,'Пудру сахара постепенно всыпать в сметану, взбивая на низкой скорости, просто чтобы сахар вмешался.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',16,NULL,'Заварную часть постепенно добавить к сметане и ещё раз взбить до объединения. Крем на данном этапе может быть немного жидким, в холодильнике он уплотнится.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',17,NULL,'Цикорий заварить в 1 стакане горячей воды. Немного остудить, но оставить тёплым, так он лучше пропитает бисквит.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',18,NULL,'Печенье полностью окунуть в цикорий, выложить на дно формы.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',19,NULL,'Сверху печенья распределить слой крема.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',20,NULL,'Выложить второй слой печенья, сверху распределить оставшийся крем.',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',21,NULL,'Поставить торт в холодильник на ночь или хотя бы на пару часов для пропитки и стабилизации. Украсить сверху оставшимся кремом (отсадить из мешка).',NULL),
	 ('d60ca44b-1155-5291-8835-2e94e51a80f6',22,NULL,'Посыпать сверху порошком какао или кэроба с помощью мелкого сита.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',1,NULL,'Все ингредиенты объединить в сотейнике при помощи венчика.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',2,NULL,'Поставить на огонь, нагреть на среднем огне до кипения, постоянно помешивая.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',3,NULL,'Снять с огня, перелить в миску, накрыть плёнкой/пакетом/пергаментом в контакт.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',4,NULL,'Остудить и поставить в холодильник минимум на 3 часа до полного охлаждения.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',5,NULL,'Клубнику измельчить блендером до состояния пюре.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',6,NULL,'Все ингредиенты соединить в сотейнике и перемешать венчиком.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',7,NULL,'Поставить на огонь, нагреть на среднем огне до кипения, постоянно помешивая.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',8,NULL,'Снять с огня, перелить в миску, накрыть плёнкой/пакетом/пергаментом в контакт.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',9,NULL,'Остудить и поставить в холодильник минимум на 3 часа до полного охлаждения.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',10,NULL,'Разогреть духовку до 180 градусов, противень застелить пергаментом. Подготовить кондитерский мешок для формирования савоярди.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',11,NULL,'Муку и разрыхлитель смешать венчиком в миске до однородности. Если есть комочки - просеять через сито.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',12,NULL,'Аквафабу поместить в высокую миску, начать взбивать.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',13,NULL,'Когда аквафаба приобретёт вид однородной мягкой "пивной" пены, влить сок лимона и продолжить взбивать на максимальной скорости.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',14,NULL,'Взбивать активно до состояния устойчивых пиков. На это может уйти до 5-10 минут. Аквафаба должна стать упругой и сопротивляться, когда вы водите по ней венчиком.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',15,NULL,'Начать вливать тонкой струйкой сироп, продолжая взбивать. Влить весь сироп в течение 1 минуты и ещё немного взбить. Меренга должна быть устойчивой и пластичной.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',16,NULL,'Всыпать смесь муки в меренгу в 3 захода, каждый раз аккуратно перемешивая лопаткой, движениями снизу вверх. Не мешайте слишком долго, чтобы меренга не опала. Если тесто получается слишком жидкое, добавьте немного рисовой муки. Тесто должно сохранять форму ',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',17,NULL,'Переложить готовое тесто в кондитерский мешок, отрезать кончик и отсадить печенье в форме савоярди. Поставить в духовку ~ на 15 минут. Готовое печенье упругое при надавливании, может быть румяное по краям, но в центре остаётся светлым. Остудить перед сборк',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',18,NULL,'Заварной крем размягчить венчиком, при необходимости можно использовать миксер, чтобы разбить комочки. Блендером пробивать нельзя, иначе крем станет жидким.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',19,NULL,'Сметану переложить в миску, удобную для взбивания. Взбивать, постепенно увеличивая скорость, до насыщения воздухом и уплотнения. На сметане должен появиться объёмный рисунок.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',20,NULL,'В заварную часть постепенно добавить взбитую сметану, аккуратно перемешивая до однородности и сохраняя максимум воздушности. Крем на данном этапе может быть немного жидким, в холодильнике он уплотнится.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',21,NULL,'Клубнику (100 г) с апельсиновым соком и сиропом измельчить блендером до однородности.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',22,NULL,'Печенье полностью окунуть в пюре, выложить на дно формы.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',23,NULL,'Сверху печенья распределить слой крема.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',24,NULL,'Сверху крема распределить слой джема (можно также положить кусочки ягод).',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',25,NULL,'Выложить второй слой печенья, сверху распределить оставшийся крем.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',26,NULL,'Поставить торт в холодильник на ночь или хотя бы на пару часов для пропитки и стабилизации. Украсить сверху оставшимся кремом (отсадить из мешка) и клубникой или джемом. 2 вариант. Фрезье',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',27,NULL,'Клубнику с апельсиновым соком и сиропом измельчить блендером до однородности.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',28,NULL,'Печенье полностью окунуть в пюре, выложить на дно формы.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',29,NULL,'Свежую клубнику разрезать пополам, выложить на печенье по периметру формы срезом к бортикам.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',30,NULL,'Заполнить пространства между клубникой и поверх печенья кремом.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',31,NULL,'Распределить слой джема поверх крема.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',32,NULL,'Выложить второй слой печенья, сверху распределить оставшийся крем.',NULL),
	 ('8a903770-e628-5530-9fbd-1c87d13b544f',33,NULL,'Сверху украсить пиками крема и свежей клубникой или джемом.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',1,NULL,'Сметану переложить в миску, удобную для взбивания',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',2,NULL,'Начать взбивать на низкой скорости, постепенно увеличивая. Взбивать в течение 2-3 минут до максимальной воздушности. На сметане должен появиться объёмный рисунок от миксера',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',3,NULL,'Во взбитую сметану добавить сгущёнку, ещё раз быстро взбить, чтобы сгущёнка полностью вмешалась. После этого дополнительно аккуратно домешать лопаткой, собирая сметану со дна и стенок',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',4,NULL,'Готовый крем сразу использовать для сборки или можно убрать в холодильник для стабилизации',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',5,NULL,'Положить первый блин лицевой стороной вниз на пергамент или плёнку (для удобства сворачивания)',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',6,NULL,'Распределить крем равномерно по площади блина, отступая немного от края (толщину крема можете регулировать по своему вкусу)',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',7,NULL,'Сверху положить блин с нахлёстом на 1/2-2/3 нижнего блина. Также намазать кремом',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',8,NULL,'Повторить со всеми 5-6 блинами и кремом',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',9,NULL,'Сверху змейкой выдавить сгущёнку (по желанию и по вкусу)',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',10,NULL,'Банан нарезать кружочками и выложить дорожками. Орехи нарубить и посыпать всю поверхность',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',11,NULL,'Подвернуть рулет по краям со всех сторон. И начать аккуратно сворачивать от себя, придерживая края, чтобы они не развернулись',NULL),
	 ('6b7aa077-fb0f-5b47-a231-509d7f72595e',12,NULL,'Убрать в холодильник минимум на 2 часа или на ночь, чтобы блины пропитались, и крем стабилизировался. После этого можно полить сгущёнкой, украсить остатками крема и орехами',NULL),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae',1,NULL,'Налить в сотейник молоко, добавить подсластитель',NULL),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae',2,NULL,'Поставить на огонь, нагреть до кипения',NULL),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae',3,NULL,'Убавить огонь на средний или чуть выше среднего и уваривать в течение 25-30 минут. Объём должен уменьшиться примерно в 2 раза. Ближе к концу проверяйте состояние, и можно немного перемешивать, чтобы масса не пригорала',NULL),
	 ('e59d307c-2fe0-5396-8137-793fc3d38bae',4,NULL,'Перелить массу в миску или баночку и убрать в холодильник до застывания и загущения Можно также заморозить порционно по формочкам и хранить до 3 месяцев в морозилке',NULL),
	 ('f0330825-3177-5e8e-9956-796217d9dc47',1,NULL,'Сухие ингредиенты тщательно смешать в миске при помощи венчика, чтобы они равномерно распределились между собой. Если мука комкуется, просейте её предварительно через сито',NULL),
	 ('f0330825-3177-5e8e-9956-796217d9dc47',2,NULL,'Добавить в смесь молоко и сироп. Начните с 600 г молока/воды. Оставить смесь загустеть на 30-40 минут, чтобы мука впитала жидкость',NULL),
	 ('f0330825-3177-5e8e-9956-796217d9dc47',3,NULL,'Проверить консистенцию, добавить воды при необходимости, если тесто слишком густое. От этого будет зависеть также толщина блинов',NULL),
	 ('f0330825-3177-5e8e-9956-796217d9dc47',4,NULL,'Сковороду разогреть и печь блины на огне чуть выше среднего (выбирайте режим, подходящий под вашу сковороду). Блины складывать стопкой, в конце можно накрыть тарелкой или полотенцем, чтобы они размягчились',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('7e931153-f384-589b-92be-b6cb7455a603',1,NULL,'Муку, крахмал и кэроб смешать венчиком, при необходимости просеять через сито (если есть комочки)',NULL),
	 ('7e931153-f384-589b-92be-b6cb7455a603',2,NULL,'Влить в сухую смесь жидкость и сироп, перемешать венчиком до однородности',NULL),
	 ('7e931153-f384-589b-92be-b6cb7455a603',3,NULL,'Оставить тесто на 20-30 минут, затем проверить текстуру. Тесто должно быть как классическое на блины, текучее, но не слишком жидкое. Если тесто слишком густое, добавьте ещё воду',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',1,NULL,'Заранее разогреть духовку до 160-170 градусов (режим верх-низ без конвекции, если это возможно), подготовить формы',NULL),
	 ('7e931153-f384-589b-92be-b6cb7455a603',4,NULL,'Разогреть сковороду и печь блины с двух сторон на огне чуть выше среднего, чтобы они не пригорали и равномерно пропекались. Подберите температуру под свою сковороду',NULL),
	 ('7e931153-f384-589b-92be-b6cb7455a603',5,NULL,'Выложить блины стопкой, накрыть сверху тарелкой/миской, чтобы избежать засыхания, и оставить до полного остывания',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',1,NULL,'Киноа замочить в холодной воде на ночь или в горячей воде на 2 часа Это сделает крупу мягче и уберёт лишний вкус',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',2,NULL,'Крупу промыть, положить в мощный блендер',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',3,NULL,'Добавить молоко, муку и соду',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',4,NULL,'Взбить до максимально гладкой консистенции в течение примерно 1 минуты',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',5,NULL,'Перелить тесто в миску для удобства или оставить в блендере. Проверить консистенцию, при необходимости добавить больше жидкости',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',6,NULL,'В конце добавить в тесто лимонный сок, чтобы погасить соду, перемешать',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',7,NULL,'На сухую разогретую сковороду (температуру подбирайте под свою плиту и сковородку, я ставлю выше средней) вылить тесто',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',8,NULL,'Жарить до румяного состояния, когда края сами легко начнуть отходить от сковороды (степень прожарки можно регулировать по своему вкусу)',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',9,NULL,'Перевернуть и дожарить со второй стороны',NULL),
	 ('7bac4627-c817-5023-8d01-5bd1d8abd966',10,NULL,'Складывать блины стопкой, можно также накрыть сверху тарелкой, чтобы блины размягчились',NULL),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',1,NULL,'Семена льна измельчить в кофемолке до состояния муки. Залить водой и оставить, пока смешиваете остальные ингредиенты',NULL),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',2,NULL,'Муку и крахмал смешать венчиком, влить молоко и сироп, перемешать до однородной консистенции',NULL),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',3,NULL,'Добавить льняное яйцо, перемешать. Оставить на 15-20 минут, чтобы мука впитала жидкость',NULL),
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',4,NULL,'Проверить консистенцию. Тесто должно быть достаточно текучее тесто, как на классические блинчики. Если тесто слишком густое, добавить ещё молоко или воду',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('4f857c5a-befd-51fc-ba5c-1cb0a9de9e64',5,NULL,'Сковороду хорошо раскалить и жарить на огне выше среднего (выбирайте режим, подходящий под вашу сковороду). Блины складывать стопкой, в конце можно накрыть тарелкой или полотенцем, чтобы они размягчились',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',1,NULL,'Подготовим шпинат Если используете замороженный, его необходимо разморозить, лишнюю воду слить, а затем потушить на сковороде до испарения влаги. Можно также разморозить прямо на сковороде. Если используете свежий: порубить и припустить на сковороде с доба',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',2,NULL,'Добавить в шпинат соль по вкусу, сушёный чеснок и мускатный орех',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',3,NULL,'Смешать шпинат с рикоттой до однородности, скорректировать при необходимости на соль и специи',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',4,NULL,'Выложить начинку на край блина полоской и завернуть блин в трубочку, подогнув немного края',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',5,NULL,'Для соуса "бешамель" кокосовое молоко и картофель взбить в блендере до однородности с добавлением соли по вкусу',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',6,NULL,'Для томатного соуса томаты взбить в блендере с травами и солью по вкусу',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',7,NULL,'Переходим к сборке. На дно формы для запекания вылить часть соуса "бешамель", распределить. Сверху выложить половину блинов (если делаете в 2 слоя), или все блины (если у вас большая форма, и вы делаете 1 слой)',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',8,NULL,'Блины полить соусом "бешамель", при необходимости повторить слои, и сверху завершить томатным соусом',NULL),
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',9,NULL,'По желанию можно посыпать тёртым кокосовым сыром',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('b03f6441-3b26-5ec8-9ebe-0cc7f060f91d',10,NULL,'Поставить в разогретую до 180 градусов духовку на 30-40 минут или до появления румяной корочки Чтобы успростить и ускорить процесс приготовления, можно заморозить готовые блинные трубочки. Разморозить в холодильнике, а затем собирать блюдо по рецепту.',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',1,NULL,'Муку и соду смешать тщательно венчиком. Если есть комочки, просеять через сито',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',2,NULL,'Влить молоко, перемешать, добавить подсластитель',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',3,NULL,'Оставить тесто на 3-5 минут, чтобы мука максимально впитала влагу. Проверить консистенцию. Чтобы блины лучше пропеклись, должно быть достаточно жидкое тесто как на классические блинчики',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',4,NULL,'Добавить лимонный сок, перемешать',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',5,NULL,'Яблоко натереть на крупной тёрке (можно как зелёное, так и красное), слегка отжать и вмешать в тесто',NULL),
	 ('62d45f79-fdd9-5cc5-b089-36ef0207db5a',6,NULL,'Сковороду хорошо раскалить, немного сбавить огонь и выпекать, равномерно распределяя тесто. Хорошо прожаривать с двух сторон, чтобы внутри тесто не оставалось сырое',NULL),
	 ('fcb65d25-0886-5071-a188-33d37d560668',1,NULL,'Для конвертиков: выложить начинку ближе к краю, по желанию можно добавить кокосовый сыр (если он мягкий - распределить ложкой по блину, если твёрдый - натереть на тёрке и посыпать сверху). Сложить конвертиком и обжарить на сухой сковороде (или с добавление',NULL),
	 ('fcb65d25-0886-5071-a188-33d37d560668',2,NULL,'Для мешочков: выложить начинку по центру, собрать края блина к центру, формируя складки. Закрепить луковыми перьями или другой зеленью. Можно выложить на противень и запечь 10-15 минут в духовке. В таком случае тоже можно добавить сыр',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',1,NULL,'Орехи промыть, замочить в холодной воде на ночь или в горячей воде на 1 час (здесь вода не по весу, просто чтобы полностью покрыла орехи)',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',2,NULL,'Слить воду, ещё раз промыть, при желании очистить миндаль от шкурок. После замачивания кожура легко отходит, но это опционально. Наличие кожуры влияет только на цвет и немного усиливает вкус',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',3,NULL,'Переложить в блендер, залить тёплой водой (600г) и взбить на высокой скорости в течение 30-60 секунд до максимально однородной текстуры',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',4,NULL,'Отжать молоко при помощи мешочка, перелить в кастрюлю на весах',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',5,NULL,'Определить количество лимонного сока: Для рикотты/сыра можно брать меньше - от 5 до 10% (5-10г на 100г)',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',6,NULL,'Молоко нагреть примерно до 80 градусов (не доводя до кипения). После этого влить лимонный сок, помешивая, и сразу выключить нагрев. Молоко должно свернуться и стать более густым',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',7,NULL,'Если молоко не поменяло текстуру, можно его уварить на среднем огне до более густой текстуры',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',8,NULL,'Свернувшееся молоко остудить, переложить в миску/контейнер и убрать в морозилку до полного застывания',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',9,NULL,'Подготовить миску и сито для отвешивания. Достать замороженное молоко, выложить на сито, если вы хотите мягкий сыр',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',10,NULL,'Оставить на 1-2 часа на столе, чтобы брикет быстрее оттаял, затем убрать в холодильник до полной разморозки, чтобы внутри массы не оставалось кусочков льда (можно на ночь)',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',11,NULL,'Если вам нужен более плотный творог, сито необходимо дополнительно проложить марлей или использовать мешочек для молока. Завернуть в них замороженный брикет, оставить при комнатной температуре. Затем поставить сверху пресс на всю поверхность (например, кон',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',12,NULL,'После разморозки в миску стечёт сыворотка (её можно оставить и использовать для блинчиков/оладьев). По желанию дополнительно отжать массу от остатков влаги',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',13,NULL,'Подготовить миску и сито для отвешивания. Сито проложить марлей или использовать мешочек для молока',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',14,NULL,'Свернувшееся молоко переложить на марлю или в мешочек (можно не остужать)',NULL),
	 ('c98d9f66-6d18-57f4-aa56-c0f91d98c8b6',15,NULL,'Поставить сверху пресс при необходимости и убрать в холодильник на ночь Свернувшееся молоко можно заготавливать в большем объёме и хранить в морозилке порционно, размораживая по необходимости.',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',1,NULL,'Духовку разогреть до 180 градусов',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',2,NULL,'Лук с грибами обжарить до испарения влаги, посолить, поперчить',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',3,NULL,'Картошку и молоко поместить в блендер. Добавить соль и специи по вкусу. Взбить до однородной консистенции',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',4,NULL,'Влить соус к грибам и уваривать до желаемой густоты (примерно как на видео). После запекания масса ещё немного загустеет',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',5,NULL,'Распределить массу по формам, при желании посыпать тёртым сыром',NULL),
	 ('b6831d0e-241c-58fe-a6a3-988e32d0241b',6,NULL,'Поставить в духовку на 20-30 минут, чтобы начинка схватилась и покрылась румяной корочкой. Если вы делаете в одной форме, времени может потребоваться больше Батат натереть на мелкой тёрке, распределить по формам, запечь 10-12 минут, чтобы корзиночки подрум',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('da9c1cec-6648-54ac-9977-5658237b1608',1,NULL,'Овсяные хлопья и пшено измельчить в муку',NULL),
	 ('da9c1cec-6648-54ac-9977-5658237b1608',2,NULL,'Муку и соду смешать венчиком до однородности',NULL),
	 ('da9c1cec-6648-54ac-9977-5658237b1608',3,NULL,'Добавить пюре и сироп, замешать тесто. При необходимости добавить немного воды (ориентируйтесь на консистенцию) Если тесто будет более жидким, это будет больше похоже на оладьи. Регулируйте количество жидкости под себя.',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',2,NULL,'В отдельной миске смешать все сухие ингредиенты, предварительно просеяв через сито (из первого столбца), перемешать венчиком',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',3,NULL,'В другой миске смешать апельсиновый сок, молоко, сахар, масло и цедру',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',4,NULL,'Объединить две смеси венчиком. Оставить на 10 минут для набухания',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',5,NULL,'Проверить текстуру. Тесто не должно быть слишком жидкое, если взять лопаткой, то оно не стекает, а медленно спадает. Если тесто слишком жидкое, добавьте по чуть-чуть каждого вида муки',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',6,NULL,'Влить лимонный сок, перемешать. После добавления сока не оставляйте тесто надолго',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',7,NULL,'Добавить начинку и перемешать до равномерного распределения',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',8,NULL,'Выложить тесто в формы примерно на 3/4, поставить в разогретую духовку примерно на 40 минут для маленьких форм и на 60 минут для больших',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',9,NULL,'Проверить готовность кулича можно легкими нажатиями сверху и по бокам. Он должен быть плотным, упругим, не проминаться. Также можно проверить при помощи деревянной шпажки, она будет выходить сухая или с небольшими кусочками теста, но не мокрая. В данном сл',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',10,NULL,'Обязательно дать куличу полностью остыть, можно оставить на ночь. Затем извлечь из формы и украсить',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',11,NULL,'Сахар измельчить в пудру при помощи кофемолки или блендера до максимально мелкого помола',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',12,NULL,'Постепенно добавлять лимонный сок, перемешивая. Довести до желаемой консистенции, чтобы удобно было покрывать кулич, и глазурь не стекала',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',13,NULL,'Сразу нанести на кулич и украсить, эта глазурь быстро схватывается и застывает',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',14,NULL,'Сахар измельчить в пудру при помощи кофемолки или блендера до максимально мелкого помола',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',15,NULL,'Аквафабу взбить миксером до мягкой пены, влить лимонный сок и ещё немного взбить',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',16,NULL,'Добавить сахар частями к аквафабе, взбивать миксером до посветления массы и максимального растворения сахара. Масса не будет сильно увеличиваться в объеме',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',17,NULL,'Помадка должна быть достаточно густая, чтобы она не стекла с кулича. Если масса получается слишком жидкая, добавить больше пудры',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',18,NULL,'Помадка достаточно быстро покрывается корочкой, поэтому наносите на кулич и украшайте сразу, или можно накрыть её плёнкой в контакт и оставить на какое-то время',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',19,NULL,'Аквафабу поместить в высокую ёмкость (можно в стакан от погружного блендера), начать взбивать миксером',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',20,NULL,'Когда взобьётся до мягких пиков, влить лимонный сок, продолжая взбивать 1-3 минуты до более плотных пиков',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',21,NULL,'Всыпать камедь дождиком, взбивать до максимально устойчивых пиков и плотного состояния',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',22,NULL,'Не останавливая миксер медленно влить тонкой струйкой сироп. Взбить ещё 1-2 минуты. Меренга должна быть очень плотная',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',23,NULL,'Готовой меренгой обмазать кулич хаотичными мазками или окунуть в неё кулич',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',24,NULL,'Готовый марципан смешать с красителями до желаемого цвета',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',25,NULL,'Далее можно раскатать скалкой на пергаменте/силиконовом коврике и вырезать детали при помощи плунжера (например, листики, цветочки), либо слепить вручную желаемые детали, в том числе яички',NULL),
	 ('9930ac04-611b-5c13-bae7-46aee48f63e7',26,NULL,'Оставить подсохнуть на столе, либо сразу украсить кулич',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',1,NULL,'Кешью замочить на ночь в холодной воде или в кипятке на 30 минут - 1 час',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',2,NULL,'После размягчения кешью хорошо промыть, отмерить 220 г, положить в блендер',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',3,NULL,'Добавить все остальные ингредиенты в чашу блендера (урбеч при необходимости растопить), взбить до однородной текстуры. Если нужно, помогать толкателем. Масса будет достаточно густая и вязкая',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',4,NULL,'Попробуйте на вкус, чтобы при необходимости скорректировать сладость и кислинку под себя',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',5,NULL,'Сухофрукты нарезать, орехи нарубить крупно и вмешать в массу',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',6,NULL,'Пасочницу проложить изнутри влажной марлей (чтобы она лучше прилегала к стенкам) в 2 слоя',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',15,NULL,'Положите часть изделий в воду. В начале периодически перемешивайте, чтобы они не прилипали ко дну',NULL),
	 ('d778de10-836d-5124-86e2-5abbe2c0c0b2',7,NULL,'Выложить массу в форму и поставить в холодильник на ночь для уплотнения и стабилизации Также можно после заморозки извлечь из формы, завернуть в плёнку и хранить в морозилке до 2 месяцев. Или оставить прямо в форме, завёрнутой в плёнку.',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',1,NULL,'Орехи замочить в холодной воде на ночь или в горячей воде на 1 час',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',2,NULL,'Размягчённые орехи промыть и поместить в чашу стационарного блендера, добавить воду, взбить в течение 1-2 минут, чтобы орехи максимально выделили белки в воду',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',3,NULL,'Перелить в мешочек для молока и отжать, чтобы жмых был максимально сухой',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',4,NULL,'Молоко перелить в кастрюлю и поставить на плиту. Добавить цедру и ваниль по желанию',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',5,NULL,'Нагревать, помешивая, до первых признаков кипения',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',6,NULL,'Как только молоко начнёт закипать, влить в него лимонный сок и перемешать. Молоко начнёт сворачиваться и густеть. Проварить ещё несколько секунд',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',7,NULL,'Снять молоко с огня и остудить до комнатной температуры',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',8,NULL,'Переложить в мешочек и отвесить 2-3 часа, чтобы стекла основная сыворотка. Можно периодически помогать, слегка отжимая руками, чтобы ускорить процесс',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',9,NULL,'В готовую рикотту добавить сахар, перемешать до растворения крупинок сахара',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',10,NULL,'Сухофрукты нарезать, орехи нарубить крупно и вмешать в массу',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',11,NULL,'Пасочницу проложить марлей в 2 слоя по желанию. В целом, масса будет хорошо отходить от формы и без марли. Если вы не используете марлю, необходимо подложить под форму пергамент или плёнку',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',12,NULL,'Выложить массу в пасочницу и поставить в морозилку до полного застывания',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',13,NULL,'После заморозки поставить форму на сито/решётку, подложив миску или другую ёмкость для стекания сыворотки. Поставить сверху пресс и убрать в холодильник на ночь (можно оставить на 1 час при комнатной температуре для более быстрой разморозки, затем убрать в',NULL),
	 ('fcd1fe6d-5e70-53e5-9f6f-582e68f71d70',14,NULL,'За ночь сыворотка стечёт вниз, масса уплотнится, после чего её можно достать из формы Также можно на этапе заморозки хранить в морозилке до 2 месяцев.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',1,NULL,'Сухие ингредиенты тщательно смешать в миске при помощи венчика, чтобы они равномерно распределились между собой.',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',2,NULL,'В смесь влить воду, начать перемешивать лопаткой/ ложкой, затем домешать руками. Должно получиться эластичное плотное тесто, которое не липнет к рукам.',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',3,NULL,'Если тесто слишком тугое или крошится и не собирается в шар, добавьте ещё немного воды Готовое тесто использовать сразу.',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',4,NULL,'Если не раскатываете сразу, заверните в плёнку или положите в пакет, чтобы оно не заветривалось. Не оставляйте тесто дольше, чем на час, так как оно высыхает',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',5,NULL,'Для раскатки теста понадобится силиконовый коврик и скалка',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',6,NULL,'Раскатывайте не больше одной порции теста за раз, если вы делаете несколько порций сразу. Остальное тесто оставляйте завёрнутым в плёнку или пакет',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',7,NULL,'Раскатывайте толщиной примерно 2 мм, равномерно по всей площади. Не раскатывайте слишком тонко, так тесто может рваться, и будет нарушаться баланс вкусов',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',8,NULL,'Не присыпайте мукой, так как с ней тесто будет плохо лепиться',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',9,NULL,'В процессе периодически переворачивайте тесто Вырежьте кружочки подходящего размера вырубкой/ стаканом. Мне больше всего нравится d 6-7см. Или нарежьте на квадраты для равиолей',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',10,NULL,'Остатки теста соберите и положите под плёнку',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',11,NULL,'Кружочки переверните обратной стороной (так тесто будет лучше слепляться), выложите в центр начинку (примерно 1 ч.л.), соедините края и тщательно слепите их между собой. Слепите желаемую форму',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',12,NULL,'Если вы делаете равиоли, положите начинку на один квадрат/кружок, накройте сверху вторым. Соедините края сначала пальцем, затем сделайте узор при помощи вилки. Можно также использовать формы для упрощения и ускорения процесса',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',13,NULL,'Если в какой-то момент тесто начинает лепиться хуже, значит у вас оно пересыхает, и нужно переворачивать не все кружочки сразу',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',14,NULL,'Доведите воду до кипения, подсолите',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',17,NULL,'После того, как изделия всплывут, варите ещё 1 минуту, если они свежеприготовленные, и 2 минуты, если замороженные',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',18,NULL,'Чтобы заморозить изделия, выложите их на доску, застеленную пергаментом или пищевой плёнкой и уберите в морозилку на 1 час',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',19,NULL,'Затем переложите изделия в зип-пакет или контейнер с крышкой, чтобы не было доступа воздуха',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',20,NULL,'Храните до 2 месяцев',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',21,NULL,'Готовые изделия можно также хранить в холодильнике до 3 суток под плёнкой или в контейнере, без доступа воздуха',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',22,NULL,'Картофель отварить на пару. После готовности сразу размять при помощи вилки или толкушки без добавления воды до состояния однородного пюре. Добавить соль (примерно 1/3 ч.л.)',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',23,NULL,'Лук и грибы мелко нарезать. Обжарить сначала лук на сухой сковороде, при необходимости добавляя воду. Добавить грибы, соль (1/3 ч.л.), перец и жарить на среднем огне до полного выпаривания влаги.',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',24,NULL,'Перемешать пюре с грибами и луком. Попробовать, при необходимости добавить ещё соль или перец. Начинка должна быть чуть более солёная, чем привычно.',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',25,NULL,'Лук мелко нарезать, чеснок измельчить. Обжарить на сухой сковороде, при необходимости добавляя немного воды. Добавить все специи и соль',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',26,NULL,'Капусту мелко нарубить, добавить к луку, немного обжарить, добавить томатную пасту',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',27,NULL,'Постепенно вливать воду и тушить на среднем огне до готовности капусты',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',28,NULL,'Фасоль измельчить в чаше блендера с s-образным лезвием, смешать с обжаркой',NULL),
	 ('6c506216-411c-525c-b0c0-f496cdf05d06',29,NULL,'Количество специй зависит от их насыщенности и вашего вкуса, поэтому пробуйте и регулируйте под себя',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',1,NULL,'Все виды муки и соль смешать венчиком. Если мука с комочками, просеять через сито.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',2,NULL,'Влить молоко комнатной температуры в смесь муки, тщательно перемешать венчиком.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',3,NULL,'Добавить сироп, перемешать.',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',4,NULL,'Оставить тесто на 20-40 минут, чтобы мука набухла.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',5,NULL,'Картошку натереть на мелкой тёрке и отжать от лишней влаги.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',6,NULL,'Зелень мелко нарубить.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',7,NULL,'По истечении времени проверить густоту теста. Если слишком густое - добавьте немного воды. Оно должно быть текучее, как классическое блинное тесто, примерно как на видео. Попробуйте на вкус, при необходимости добавьте соль или сироп.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',8,NULL,'Добавить начинку в тесто и хорошо перемешать.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',9,NULL,'Сковороду разогреть и печь блины на среднем огне или чуть выше среднего, чтобы начинка и тесто пропеклись.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',10,NULL,'Помогайте распределять начинку и тесто по сковороде с помощью половника или ложки.',NULL),
	 ('9bdd84a3-6cae-5790-ba26-f48bb566f60c',11,NULL,'Печь с двух сторон и складывать блины стопочкой, чтобы они рязмягчились. Если хотите хрустящую корочку, кладите в тарелку по одному и ешьте сразу В морозилке хранить до 2 месяцев в герметичном контейнере или пакете. Разморозить можно в холодильнике, при ко',NULL),
	 ('1755d74c-998a-545b-b725-0439d99037fb',1,NULL,'Муку смешать в миске венчиком',NULL),
	 ('1755d74c-998a-545b-b725-0439d99037fb',2,NULL,'Влить жидкость и тщательно перемешать',NULL);
INSERT INTO public."RecipeSteps" ("RecipeId","SeqNumber","PhotoId","Description","Comment") VALUES
	 ('1755d74c-998a-545b-b725-0439d99037fb',3,NULL,'Оставить тесто на 20-30 минут, чтобы мука набухла',NULL),
	 ('1755d74c-998a-545b-b725-0439d99037fb',4,NULL,'После этого проверить консистенцию. Тесто должно быть текучее, как классическое блинное. Если оно слишком густое, добавьте ещё воды или молока',NULL),
	 ('1755d74c-998a-545b-b725-0439d99037fb',5,NULL,'Выпекать на разогретой сковороде на огне выше среднего. Складывать блины стопочкой, чтобы они были мягкими Также возможно хранение в морозилке в герметичном контейнере или пакете до 2 месяцев. Размораживать при комнатной температуре, в холодильнике или дух',NULL);
