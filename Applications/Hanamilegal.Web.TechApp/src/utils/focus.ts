export function removeNavLinkActive(timeout: number = 100): void {
  const activeClass: string = "active";
  const navLinkClass: string = `.nav-link.${activeClass}`;

  setTimeout(() => {
    const navLink: HTMLElement | null = document.querySelector(navLinkClass) as HTMLElement | null;
    navLink?.classList.remove(activeClass);
  }, timeout);
}
