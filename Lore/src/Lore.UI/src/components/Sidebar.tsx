'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { Film, Globe } from 'lucide-react'

const NAV = [
  { href: '/movies', label: 'Movies', icon: Film },
  { href: '/universes', label: 'Universes', icon: Globe },
]

export function Sidebar() {
  const pathname = usePathname()
  return (
    <aside className="w-56 shrink-0 bg-gray-900 min-h-screen flex flex-col">
      <div className="px-5 py-5 border-b border-gray-800">
        <span className="text-lg font-bold text-white tracking-tight">Lore</span>
      </div>
      <nav className="flex-1 px-3 py-4 flex flex-col gap-1">
        {NAV.map(({ href, label, icon: Icon }) => {
          const active = pathname === href || (href !== '/' && pathname.startsWith(href + '/')) || (href === '/movies' && pathname === '/movies')
          return (
            <Link
              key={href}
              href={href}
              className={`flex items-center gap-3 px-3 py-2.5 rounded-xl text-sm font-medium transition-colors ${
                active
                  ? 'bg-indigo-600 text-white'
                  : 'text-gray-400 hover:text-white hover:bg-gray-800'
              }`}
            >
              <Icon className="w-4 h-4 shrink-0" />
              {label}
            </Link>
          )
        })}
      </nav>
    </aside>
  )
}
