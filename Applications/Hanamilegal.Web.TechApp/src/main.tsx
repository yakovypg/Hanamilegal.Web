import "i18n";
import "bootstrap/dist/css/bootstrap.min.css";

import { ErrorBoundary } from "components";
import { Layout } from "layout";
import { ModalProvider } from "modals";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { ROUTES } from "routes";

const root: HTMLElement | null = document.getElementById("root");

createRoot(root!).render(
  <BrowserRouter>
    <ModalProvider>
      <ErrorBoundary>
        <Layout>{ROUTES}</Layout>
      </ErrorBoundary>
    </ModalProvider>
  </BrowserRouter>
);
