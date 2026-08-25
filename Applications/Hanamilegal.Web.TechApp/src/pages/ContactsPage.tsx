import "styles/pages/contacts-page.css";

import contactsConfig from "@config/contacts.json";
import { ContactsMenu } from "components";
import React from "react";
import { useTranslation } from "react-i18next";

export const ContactsPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div>
      <h3 className="text-center text-decoration-underline">{t("title.contacts")}</h3>
      <ContactsMenu className="mt-4" />
      <div className="mt-4 text-center">
        <p className="m-0">{t("organization.name")}</p>
        <p className="m-0">{t("organization.address")}</p>
      </div>
      {/* <div className="yandex-map-container">
        <YandexMap className="yandex-map" address={contactsConfig.yandexMapAddress} />
      </div> */}
      <div className="mt-3 text-center">
        <a href={contactsConfig.yandexMapLink}>{t("sentense.openInMap")}</a>
      </div>
    </div>
  );
};
