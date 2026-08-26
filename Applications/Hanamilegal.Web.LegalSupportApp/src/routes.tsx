import { ErrorPage, HomePage, NotFoundPage } from "pages";
import { Navigate, Route, Routes } from "react-router-dom";
import { VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

export const ROUTES: React.ReactNode = (
  <Routes>
    <Route index path={getRoutePath(VIEW_PANELS.root)} element={<HomePage />} />
    <Route path={getRoutePath(VIEW_PANELS.error)} element={<ErrorPage />} />
    <Route path={getRoutePath(VIEW_PANELS.home)} element={<HomePage />} />
    <Route path={getRoutePath(VIEW_PANELS.notFound)} element={<NotFoundPage />} />
    <Route path="*" element={<Navigate to={getRoutePath(VIEW_PANELS.notFound)} replace />} />
  </Routes>
);
