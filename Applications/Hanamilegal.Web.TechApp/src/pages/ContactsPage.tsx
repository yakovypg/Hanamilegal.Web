import "styles/pages/contacts-page.css";

import contactsConfig from "@config/contacts.json";
import { ContactsMenu } from "components";
import { useModal } from "modals";
import React from "react";
import { useTranslation } from "react-i18next";

export const ContactsPage: React.FC = () => {
  const { t } = useTranslation();
  const { openModal, closeModal } = useModal();

  const openTestModal = () =>
    openModal({
      content: (
        <div className="p-5 text-center">
          <h3>(^.^) (^-^) (^.^)</h3>
          <div>
            <img
              className="my-4 w-100"
              src="https://media3.giphy.com/media/v1.Y2lkPTZjMDliOTUyNHp2MHQ5dXgzYWh3dWJyMDFpaHR0cTRhNTNicTFsbXIzOG1vMGhrZyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/h72H0tXcBjqI6edm6M/giphy.gif"
            />
          </div>
          <button onClick={closeModal}>X</button>
        </div>
      ),
      closeAfterClickOnBlurredArea: true
    });

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
      <div className="mt-3 text-center">
        <button type="button" onClick={openTestModal} className="btn btn-primary">
          (^‿^)
        </button>
      </div>
    </div>
  );
};
