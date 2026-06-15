'use client'

import { createContext, useContext, useEffect, useState } from 'react'
import {
  type AdminLang,
  type Strings,
  ADMIN_LANG_KEY,
  DEFAULT_ADMIN_LANG,
  translations,
} from '@/lib/i18n'

interface AdminLanguageContextValue {
  lang: AdminLang
  strings: Strings
  setLang: (lang: AdminLang) => void
}

const AdminLanguageContext = createContext<AdminLanguageContextValue | null>(null)

export function AdminLanguageProvider({ children }: { children: React.ReactNode }) {
  const [lang, setLangState] = useState<AdminLang>(DEFAULT_ADMIN_LANG)

  useEffect(() => {
    const stored = localStorage.getItem(ADMIN_LANG_KEY) as AdminLang | null
    if (stored && stored in translations) {
      setLangState(stored)
    }
  }, [])

  const setLang = (next: AdminLang) => {
    setLangState(next)
    localStorage.setItem(ADMIN_LANG_KEY, next)
  }

  return (
    <AdminLanguageContext.Provider value={{ lang, strings: translations[lang], setLang }}>
      {children}
    </AdminLanguageContext.Provider>
  )
}

export function useAdminStrings() {
  const ctx = useContext(AdminLanguageContext)
  if (!ctx) {
    throw new Error('useAdminStrings must be used inside AdminLanguageProvider')
  }
  return ctx
}
