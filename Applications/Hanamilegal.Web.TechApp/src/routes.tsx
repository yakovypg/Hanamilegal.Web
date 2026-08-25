import { ServiceDetailWrapper } from "components";
import {
  ContactsPage,
  ErrorPage,
  HomePage,
  NotFoundPage,
  ProjectsPage,
  ReviewsPage,
  ServicesPage
} from "pages";
import { Navigate, Route, Routes } from "react-router-dom";
import { VIEW_PANELS } from "types";
import { getRoutePath, getRoutePathWithSlug } from "utils";

export const ROUTES: React.ReactNode = (
  <Routes>
    <Route index path={getRoutePath(VIEW_PANELS.root)} element={<HomePage />} />
    <Route path={getRoutePath(VIEW_PANELS.contacts)} element={<ContactsPage />} />
    <Route path={getRoutePath(VIEW_PANELS.error)} element={<ErrorPage />} />
    <Route path={getRoutePath(VIEW_PANELS.home)} element={<HomePage />} />
    <Route path={getRoutePath(VIEW_PANELS.notFound)} element={<NotFoundPage />} />
    <Route path={getRoutePath(VIEW_PANELS.projects)} element={<ProjectsPage />} />
    <Route path={getRoutePath(VIEW_PANELS.reviews)} element={<ReviewsPage />} />
    <Route path={getRoutePath(VIEW_PANELS.services)} element={<ServicesPage />} />
    <Route path={getRoutePathWithSlug(VIEW_PANELS.services)} element={<ServiceDetailWrapper />} />
    <Route path="*" element={<Navigate to={getRoutePath(VIEW_PANELS.notFound)} replace />} />
  </Routes>
);
