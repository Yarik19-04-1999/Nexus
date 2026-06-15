'use client'

import { type AdminLang, LANG_LABELS } from '@/lib/i18n'
import { useAdminStrings } from './AdminLanguageContext'

const LANGS: AdminLang[] = ['ru', 'uk', 'en']

export function LanguageSelector() {
  const { lang, setLang } = useAdminStrings()

  return (
    <div className="flex items-center gap-1 rounded-xl border border-gray-200 bg-white px-1 py-1">
      {LANGS.map((l) => (
        <button
          key={l}
          onClick={() => setLang(l)}
          className={`px-3 py-1 rounded-lg text-xs font-medium transition-colors ${
            lang === l
              ? 'bg-emerald-500 text-white'
              : 'text-gray-500 hover:bg-gray-100'
          }`}
        >
          {LANG_LABELS[l]}
        </button>
      ))}
    </div>
  )
}
